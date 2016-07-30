using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace ReactiveUI.Routing.Android
{
    public class AndroidActivityPresenter : AndroidPresenter
    {
        public class FuncDisposable : IDisposable
        {
            private readonly Action action;

            public FuncDisposable(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action();
            }
        }

        private AndroidActivityCallbacks ActivityCallbacks { get; }

        public AndroidActivityPresenter(Application application = null, Context context = null, IViewTypeLocator viewLocator = null)
            : base(application, context, viewLocator)
        {
            ActivityCallbacks = new AndroidActivityCallbacks();
            Application.RegisterActivityLifecycleCallbacks(ActivityCallbacks);
        }

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            var viewModelType = viewModel.GetType();
            var viewType = ViewLocator.ResolveViewType(viewModelType);
            if (viewType != null)
            {
                Context.StartActivity(viewType);
                var activity = await ActivityCallbacks.ActivityCreated
                    .FirstAsync(a => a.GetType() == viewType);
                ((IViewFor) activity).ViewModel = viewModel;
                return new ScheduledDisposable(RxApp.MainThreadScheduler,
                        new FuncDisposable(() => activity?.Finish()));
            }
            else
            {
                throw new InvalidOperationException($"Could not resolve activity for {viewModelType}. Make sure that a IViewFor<{viewModelType}> exists in the current assembly.");
            }
        }
    }
}