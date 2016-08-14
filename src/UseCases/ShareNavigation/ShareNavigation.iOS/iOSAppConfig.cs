using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;
using Splat;

namespace ShareNavigation
{
    public class iOSAppConfig : CompositeRoutedAppConfig
    {
        public iOSAppConfig(DefaultAppDelegate appDelegate) 
            : base(
                  new DefaultDependencies(), 
                  new ShareNavigationDependencies(), 
                  new DefaultiOSDependencies(appDelegate))
        {
        }
    }
}
