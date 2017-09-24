using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using ReactiveUI.Routing.Registrations;
using Splat;

namespace ReactiveUI.Routing.Android
{
    class AndroidRoutingDependencies : ReactiveAppBuilder
    {
        public AndroidRoutingDependencies()
        {
            var handler = new ActivityLifecycleCallbackHandler();

            // TODO: somehow register the handler to the activity appliaction
            // mainActivity.Application.RegisterActivityLifecycleCallbacks(handler);

            this.RegisterConstant(handler, typeof(ActivityLifecycleCallbackHandler));

            var fragmentActivationForViewFetcher = new LazyRegistration(() => 
                new FragmentActivationForViewFetcher(Locator.Current.GetService<FragmentManager>()));
            var activityActivationForViewFetcher = new LazyRegistration(() => 
                new ActivityActivationForViewFetcher(handler));

            this.Add(fragmentActivationForViewFetcher.CreateDependency(typeof(IActivationForViewFetcher)));
            this.Add(fragmentActivationForViewFetcher.CreateDependency(typeof(FragmentActivationForViewFetcher)));
            this.Add(activityActivationForViewFetcher.CreateDependency(typeof(IActivationForViewFetcher)));
            this.Add(activityActivationForViewFetcher.CreateDependency(typeof(ActivityActivationForViewFetcher)));
        }
    }
}