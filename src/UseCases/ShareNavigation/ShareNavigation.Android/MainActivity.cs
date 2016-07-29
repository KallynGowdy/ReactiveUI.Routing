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
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace ShareNavigation
{
    [Activity(Label = "MainActivity", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private SuspensionNotifierHelper suspensionNotifier;
        private IRoutedAppHost host;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            host = new RoutedAppHost(new AndroidAppConfig(Application, this));
            suspensionNotifier = Locator.Current.GetService<SuspensionNotifierHelper>();
            host.Start();
        }

        protected override void OnDestroy()
        {
            suspensionNotifier.TriggerSuspension();
            base.OnDestroy();
        }
    }
}