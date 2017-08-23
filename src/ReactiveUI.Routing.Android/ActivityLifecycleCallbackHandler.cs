using System;
using System.Reactive.Subjects;
using Android.App;
using Android.OS;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ReactiveUI.Routing.Android
{
    public class ActivityLifecycleCallbackHandler : Java.Lang.Object, Application.IActivityLifecycleCallbacks
    {
        private Subject<Activity> activityStarted = new Subject<Activity>();
        private Subject<Activity> activityStopped = new Subject<Activity>();

        //public IObservable<(Activity activity, Bundle savedInstanceState)> ActivityCreated { get; }
        //public IObservable<Activity> ActivityDestroyed { get; }
        //public IObservable<Activity> ActivityResumed { get; }
        public IObservable<Activity> ActivityStarted => activityStarted;

        public IObservable<Activity> ActivityStopped => activityStopped;

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
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
            activityStarted.OnNext(activity);
        }

        public void OnActivityStopped(Activity activity)
        {
            activityStopped.OnNext(activity);
        }
    }
}