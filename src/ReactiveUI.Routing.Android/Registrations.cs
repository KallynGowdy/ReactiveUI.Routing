using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Routing.Android
{
    internal class Registrations : IWantsToRegisterStuff
    {
        private readonly FragmentActivity mainActivity;

        public Registrations(FragmentActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void Register(IMutableDependencyResolver resolver)
        {
            ActivityLifecycleCallbackHandler handler = new ActivityLifecycleCallbackHandler();
            mainActivity.Application.RegisterActivityLifecycleCallbacks(handler);

            Locator.CurrentMutable.RegisterConstant(handler, typeof(ActivityLifecycleCallbackHandler));
            Locator.CurrentMutable.Register(() => new ActivityActivationForViewFetcher(), typeof(IActivationForViewFetcher));
            Locator.CurrentMutable.Register(() => new FragmentActivationForViewFetcher(mainActivity.SupportFragmentManager), typeof(IActivationForViewFetcher));
        }
    }
}