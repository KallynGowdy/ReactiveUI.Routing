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
        private readonly DefaultAndroidConfig defaultAndroidConfig;

        public AndroidAppConfig(Activity hostActivity, Bundle savedInstanceState)
        {
            defaultAndroidConfig = new DefaultAndroidConfig(hostActivity, savedInstanceState);
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            defaultAndroidConfig.RegisterDependencies(resolver);
        }

        public override void CloseApp() => defaultAndroidConfig.CloseApp();
        protected override ISuspensionNotifier BuildSuspensionNotifier() => defaultAndroidConfig.BuildSuspensionNotifier();
        protected override IObjectStateStore BuildObjectStateStore() => defaultAndroidConfig.BuildObjectStateStore();
    }
}