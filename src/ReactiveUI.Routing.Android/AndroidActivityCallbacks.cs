using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.App;
using Android.OS;
using Object = Java.Lang.Object;

namespace ReactiveUI.Routing.Android
{
    public class SaveInstanceStateEvent
    {
        public Activity Activity { get; set; }
        public Bundle Bundle { get; set; }
    }

    public class AndroidActivityCallbacks : Object, Application.IActivityLifecycleCallbacks
    {
        private readonly BehaviorSubject<Activity> activityCreated = new BehaviorSubject<Activity>(null);
        private readonly BehaviorSubject<SaveInstanceStateEvent> saveInstanceState = new BehaviorSubject<SaveInstanceStateEvent>(null);
        private readonly BehaviorSubject<Activity> activityPaused = new BehaviorSubject<Activity>(null);
        private readonly BehaviorSubject<Activity> activityResumed = new BehaviorSubject<Activity>(null);

        public IObservable<Activity> ActivityResumed => activityResumed.Where(a => a != null);
        public IObservable<Activity> ActivityPaused => activityPaused.Where(a => a != null);
        public IObservable<Activity> ActivityCreated => activityCreated.Where(a => a != null);
        public IObservable<SaveInstanceStateEvent> SaveInstanceState => saveInstanceState.Where(a => a != null);

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            activityCreated.OnNext(activity);
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
            activityPaused.OnNext(activity);
        }

        public void OnActivityResumed(Activity activity)
        {
            activityResumed.OnNext(activity);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            saveInstanceState.OnNext(new SaveInstanceStateEvent()
            {
                Activity = activity,
                Bundle = outState
            });
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}