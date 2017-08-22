using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReactiveUI.Routing.UseCases.Android
{
    public class Application : global::Android.App.Application, global::Android.App.Application.IActivityLifecycleCallbacks
    {
        private AutoSuspendHelper suspendHelper;
        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }


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
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            suspendHelper = new AutoSuspendHelper(this);
        }
    }
}