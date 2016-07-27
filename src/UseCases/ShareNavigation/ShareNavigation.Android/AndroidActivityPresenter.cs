using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using ReactiveUI;
using ReactiveUI.Routing;
using Object = Java.Lang.Object;

namespace ShareNavigation
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

        public class Callbacks : Object, Application.IActivityLifecycleCallbacks
        {
            private readonly BehaviorSubject<Activity> activityCreated = new BehaviorSubject<Activity>(null);
            public IObservable<Activity> ActivityCreated => activityCreated.Where(a => a != null);

            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
                activityCreated.OnNext(activity);
            }

            public void OnActivityDestroyed(Activity activity)
            {
            }

            public void OnActivityPaused(Activity activity)
            {
            }

            public void OnActivityResumed(Activity activity)
            {
            }

            public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
            {
            }

            public void OnActivityStarted(Activity activity)
            {
            }

            public void OnActivityStopped(Activity activity)
            {
            }
        }

        private Callbacks ActivityCallbacks { get; }

        public AndroidActivityPresenter(Application application = null, Context context = null, IViewTypeLocator viewLocator = null)
            : base(application, context, viewLocator)
        {
            ActivityCallbacks = new Callbacks();
            Application.RegisterActivityLifecycleCallbacks(ActivityCallbacks);
        }

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            var viewModelType = viewModel.GetType();
            var viewType = ViewLocator.ResolveViewType(viewModelType);
            if (viewType != null)
            {
                return Observable.Create<Activity>(o =>
                {
                    Activity activity = null;
                    var sub = ActivityCallbacks.ActivityCreated
                        .FirstAsync(a => a.GetType() == viewType)
                        .Do(a => activity = a)
                        .Cast<IViewFor>()
                        .Do(a => a.ViewModel = viewModel)
                        .Subscribe(a => { }, onError: o.OnError);
                    Context.StartActivity(viewType);

                    return new ScheduledDisposable(RxApp.MainThreadScheduler,
                        new CompositeDisposable(
                            sub,
                            new FuncDisposable(() => activity?.Finish())));
                }).Subscribe();
            }
            else
            {
                throw new InvalidOperationException($"Could not resolve activity for {viewModelType}. Make sure that a IViewFor<{viewModelType}> exists in the current assembly.");
            }
        }
    }
}