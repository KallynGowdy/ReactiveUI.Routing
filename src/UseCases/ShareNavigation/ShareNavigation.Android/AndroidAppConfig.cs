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
using ReactiveUI.Routing;
using ShareNavigation.Views;
using Splat;

namespace ShareNavigation
{
    public class AndroidAppConfig : RoutedAppConfig
    {
        private readonly Application application;
        private readonly Context hostActivity;

        public AndroidAppConfig(Application application, Context hostActivity)
        {
            this.application = application;
            this.hostActivity = hostActivity;
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.RegisterConstant(application, typeof(Application));
            resolver.RegisterConstant(hostActivity, typeof(Context));
            resolver.RegisterLazySingleton(() => new SuspensionNotifierHelper(), typeof(SuspensionNotifierHelper));
            resolver.Register(() => new AndroidActivityPresenter(), typeof(IPresenter));
        }

        protected override ISuspensionNotifier BuildSuspensionNotifier()
        {
            return Locator.Current.GetService<SuspensionNotifierHelper>();
        }
    }
}