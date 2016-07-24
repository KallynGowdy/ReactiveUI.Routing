﻿using System;
using Android.App;
using Android.Runtime;
using ReactiveUI;
using ReactiveUI.Routing;

namespace ShareNavigation
{
    [Application]
    public class AndroidApplication : Application
    {
        private IRoutedAppHost host;
        public AndroidApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
        {
            host = new RoutedAppHost(new AndroidAppConfig(this));
        }

        public override void OnCreate()
        {
            base.OnCreate();
            host.Start();
        }
    }
}

