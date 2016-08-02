using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Android;
using ShareNavigation.Views;
using Splat;

namespace ShareNavigation
{
    public class AndroidAppConfig : RoutedAppConfig
    {
        private readonly Activity hostActivity;

        public AndroidAppConfig(Activity hostActivity)
        {
            this.hostActivity = hostActivity;
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            AndroidRegistrations.RegisterDependencies(hostActivity, resolver);
        }

        public override void CloseApp()
        {
            hostActivity.FinishAffinity();
        }

        protected override ISuspensionNotifier BuildSuspensionNotifier()
        {
            return Locator.Current.GetService<SuspensionNotifierHelper>();
        }
    }
}