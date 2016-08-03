using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;
using Splat;

namespace ShareNavigation
{
    public class iOSAppConfig : RoutedAppConfig
    {
        private readonly DefaultiOSConfig iosAppConfig;

        public iOSAppConfig(AppDelegate appDelegate)
        {
            iosAppConfig = new DefaultiOSConfig(appDelegate);
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            iosAppConfig.RegisterDependencies(resolver);
        }

        public override void CloseApp() => iosAppConfig.CloseApp();
        protected override ISuspensionNotifier BuildSuspensionNotifier() => iosAppConfig.BuildSuspensionNotifier();
        protected override IObjectStateStore BuildObjectStateStore() => iosAppConfig.BuildObjectStateStore();
    }
}
