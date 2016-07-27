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
    [Activity(Label = "MainActivity", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private IRoutedAppHost host;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            host = new RoutedAppHost(new AndroidAppConfig(Application, this));
            host.Start();
        }
    }
}