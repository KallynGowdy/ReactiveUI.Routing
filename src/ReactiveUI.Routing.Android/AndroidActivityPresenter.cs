using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace ReactiveUI.Routing.Android
{
    public class AndroidActivityPresenter : AndroidPresenter, IPagePresenter
    {

        private AndroidActivityCallbacks ActivityCallbacks { get; }

        public AndroidActivityPresenter(Application application = null, Context context = null, IViewTypeLocator viewLocator = null)
            : base(application, context, viewLocator)
        {
            ActivityCallbacks = new AndroidActivityCallbacks();
            Application.RegisterActivityLifecycleCallbacks(ActivityCallbacks);
        }

        public override int GetAffinityForView(Type view) => typeof(Activity).IsAssignableFrom(view) ? 1000 : 0;

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            var viewModelType = viewModel.GetType();
            var viewType = ResolveViewTypeForViewModelType(viewModelType);
            Context.StartActivity(viewType);
            var activity = await ActivityCallbacks.ActivityCreated
                .FirstAsync(a => a.GetType() == viewType);
            var d = ActivityCallbacks.ActivityResumed
                .Where(a => a.GetType() == activity.GetType())
                .Select(a => (IViewFor)a)
                .Do(a => a.ViewModel = viewModel)
                .Do(NotifyViewActivated)
                .Subscribe();
            var d1 = ActivityCallbacks.ActivityPaused
                .Where(a => a.GetType() == activity.GetType())
                .Select(a => (IViewFor)a)
                .Do(NotifyViewDeActivated)
                .Subscribe();

            return new ScheduledDisposable(
                RxApp.MainThreadScheduler,
                new CompositeDisposable(
                    d,
                    d1,
                    new ActionDisposable(() => activity?.Finish())
            ));
        }
    }
}