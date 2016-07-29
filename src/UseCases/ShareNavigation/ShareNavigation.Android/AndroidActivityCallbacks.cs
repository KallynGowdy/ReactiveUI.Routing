using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace ShareNavigation
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
        }

        public void OnActivityResumed(Activity activity)
        {
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