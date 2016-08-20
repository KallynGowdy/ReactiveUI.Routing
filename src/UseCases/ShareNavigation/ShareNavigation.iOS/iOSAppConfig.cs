using System.Reflection;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;
using ShareNavigation.iOS.Views;
using ShareNavigation.ViewModels;
using Splat;

namespace ShareNavigation.iOS
{
    public class iOSAppConfig : CompositeRoutedAppConfig
    {
        public iOSAppConfig(DefaultAppDelegate appDelegate)
            : base(
                  new DefaultDependencies(Assembly.GetExecutingAssembly()),
                  new ShareNavigationDependencies(),
                  new DefaultiOSDependencies(appDelegate))
        {
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.Register(() => new PhotoListViewController(), typeof(PhotoListViewController));
        }
    }
}
