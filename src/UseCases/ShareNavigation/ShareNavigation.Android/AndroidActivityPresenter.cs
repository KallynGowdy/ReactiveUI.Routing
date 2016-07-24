using System;
using System.Collections.Generic;
using System.Linq;
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
            public IObservable<Activity> ActivityCreated => activityCreated;

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

        public AndroidActivityPresenter(Application context, IViewTypeLocator viewLocator = null) : base(context, viewLocator)
        {
            ActivityCallbacks = new Callbacks();
            Context.RegisterActivityLifecycleCallbacks(ActivityCallbacks);
        }

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            var viewType = ViewLocator.ResolveViewType(viewModel.GetType());
            if (viewType != null)
            {
                Context.StartActivity(viewType);
                var activity = await ActivityCallbacks.ActivityCreated
                    .FirstAsync(a => a.GetType() == viewType);
                return new ScheduledDisposable(RxApp.MainThreadScheduler, new FuncDisposable(() => activity?.Finish()));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}