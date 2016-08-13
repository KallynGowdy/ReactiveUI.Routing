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
    public class AndroidAppConfig : CompositeRoutedAppConfig
    {
        public AndroidAppConfig(Activity hostActivity, Bundle savedInstanceState)
            : base(
                  new DefaultDependencies(),
                  new ShareNavigationDependencies(),
                  new DefaultAndroidConfig(hostActivity, savedInstanceState))
        {
        }
    }
}