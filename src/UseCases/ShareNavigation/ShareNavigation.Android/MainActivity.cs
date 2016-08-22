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
using ReactiveUI.Routing.Android;
using Splat;

namespace ShareNavigation
{
    [Activity(Label = "Share Navigation", MainLauncher = true)]
    public class MainActivity : SuspendableAcitivity
    {
        private IRoutedAppHost host;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            host = new RoutedAppHost(new AndroidAppConfig(this, savedInstanceState));
            host.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SuspensionNotifier?.TriggerSuspension();
        }
    }
}