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
    public class DefaultiOSConfig : IRoutedAppConfig
    {
        private readonly DefaultAppDelegate appDelegate;

        public DefaultiOSConfig(DefaultAppDelegate appDelegate)
        {
            if (appDelegate == null) throw new ArgumentNullException(nameof(appDelegate));
            this.appDelegate = appDelegate;
        }

        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(appDelegate, typeof(UIApplicationDelegate));
            resolver.RegisterConstant(appDelegate, typeof(DefaultAppDelegate));
            resolver.RegisterLazySingleton(() => resolver.GetService<IObjectStateStore>(), typeof(NSCoderObjectStateStore));
        }

        public void CloseApp()
        {
        }

        public IObjectStateStore BuildObjectStateStore() => new NSCoderObjectStateStore();
        public ISuspensionNotifier BuildSuspensionNotifier() => appDelegate;
    }
}

