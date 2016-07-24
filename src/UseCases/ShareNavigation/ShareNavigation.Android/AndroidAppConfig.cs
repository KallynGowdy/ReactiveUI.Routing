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
using Splat;

namespace ShareNavigation
{
    public class AndroidAppConfig : RoutedAppConfig
    {
        private readonly Application appContext;

        public AndroidAppConfig(Application appContext)
        {
            this.appContext = appContext;
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.RegisterConstant(appContext, typeof(Application));
            resolver.Register(() => new AndroidActivityPresenter(appContext), typeof(IPresenter));
        }
    }
}