using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public class ActivityActivationForViewFetcher : IActivationForViewFetcher
    {
        private ActivityLifecycleCallbackHandler handler;

        public ActivityActivationForViewFetcher(ActivityLifecycleCallbackHandler handler = null)
        {
            this.handler = handler ?? Locator.Current.GetService<ActivityLifecycleCallbackHandler>();
        }

        public int GetAffinityForView(Type view)
        {
            if (typeof(Activity).IsAssignableFrom(view))
            {
                return 100;
            }
            return 0;
        }

        public IObservable<bool> GetActivationForView(IActivatable view)
        {
            var activity = (Activity)view;
            return Observable.Merge(
                handler.ActivityCreated.Where(args => args.activity == activity).Select(a => true),
                handler.ActivityDestroyed.Where(a => a == activity).Select(a => false));
        }
    }
}