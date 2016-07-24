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

namespace ShareNavigation
{
    public class RoutedAndroidHost : RoutedAppHost
    {
        private Context applicationContext;

        public RoutedAndroidHost(Context applicationContext, IRoutedAppConfig config)
            : base(config)
        {
            if (applicationContext == null) throw new ArgumentNullException(nameof(applicationContext));
            this.applicationContext = applicationContext;
        }
    }
}