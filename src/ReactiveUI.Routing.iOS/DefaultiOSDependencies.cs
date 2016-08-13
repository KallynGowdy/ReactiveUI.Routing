using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Foundation;
using Splat;
using UIKit;

namespace ReactiveUI.Routing.iOS
{
    /// <summary>
    /// Defines a <see cref="IRoutedAppConfig"/> that provides default iOS-specific services.
    /// </summary>
    public class DefaultiOSDependencies : IRoutedAppConfig
    {
        private readonly DefaultAppDelegate appDelegate;

        public DefaultiOSDependencies(DefaultAppDelegate appDelegate)
        {
            if (appDelegate == null) throw new ArgumentNullException(nameof(appDelegate));
            this.appDelegate = appDelegate;
        }

        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(appDelegate, typeof(UIApplicationDelegate));
            resolver.RegisterConstant(appDelegate, typeof(DefaultAppDelegate));
            resolver.RegisterConstant(appDelegate, typeof(ISuspensionNotifier));
            resolver.RegisterLazySingleton(() => new NSCoderObjectStateStore(), typeof(NSCoderObjectStateStore));
        }

        public void CloseApp()
        {
        }
    }
}

