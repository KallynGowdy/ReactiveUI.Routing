using Foundation;
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;
using ShareNavigation.iOS.Views;
using UIKit;

namespace ShareNavigation.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : DefaultAppDelegate
    {
        protected override IRoutedAppConfig BuildAppConfig(UIApplication app, NSDictionary options)
        {
            return new iOSAppConfig(this);
        }
    }
}


