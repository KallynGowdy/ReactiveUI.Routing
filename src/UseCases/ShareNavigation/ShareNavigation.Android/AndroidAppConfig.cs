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
        private readonly Context mainActivityContext;

        public AndroidAppConfig(Application application, Context mainActivityContext)
        {
            this.application = application;
            this.mainActivityContext = mainActivityContext;
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.RegisterConstant(application, typeof(Application));
            resolver.RegisterConstant(mainActivityContext, typeof(Context));
            resolver.Register(() => new AndroidActivityPresenter(), typeof(IPresenter));
        }
    }
}