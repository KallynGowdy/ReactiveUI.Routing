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
        private Subject<Activity> activityDestoyed = new Subject<Activity>();
        private Subject<Activity> activityResumed = new Subject<Activity>();
        private Subject<(Activity, Bundle)> activityCreated = new Subject<(Activity, Bundle)>();
        private Subject<(Activity, Bundle)> activitySaveInstanceState = new Subject<(Activity, Bundle)>();

        public IObservable<(Activity activity, Bundle savedInstanceState)> ActivityCreated => activityCreated;
        public IObservable<(Activity activity, Bundle outState)> ActivitySaveInstanceState => activitySaveInstanceState;

        public IObservable<Activity> ActivityDestroyed => activityDestoyed;
        public IObservable<Activity> ActivityStarted => activityStarted;
        public IObservable<Activity> ActivityStopped => activityStopped;
        public IObservable<Activity> ActivityResumed => activityResumed;

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            activityCreated.OnNext((activity, savedInstanceState));
        }

        public void OnActivityDestroyed(Activity activity)
        {
            activityDestoyed.OnNext(activity);
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            activityResumed.OnNext(activity);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            activitySaveInstanceState.OnNext((activity, outState));
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