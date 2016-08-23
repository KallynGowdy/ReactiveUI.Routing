using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;
using Xamarin.Forms;

namespace ReactiveUI.Routing.XamForms
{
    public class DefaultXamFormsDependencies : IRoutedAppConfig
    {
        private readonly RoutedApplication application;

        public DefaultXamFormsDependencies(RoutedApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            this.application = application;
        }

        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(application, typeof(Application));
            resolver.RegisterConstant(application, application.GetType());
            resolver.RegisterConstant(application, typeof(ISuspensionNotifier));
            resolver.RegisterLazySingleton(() => new NavigationPagePresenter(), typeof(NavigationPagePresenter));
            resolver.RegisterLazySingleton(() => resolver.GetService<NavigationPagePresenter>(), typeof(IActivationForViewFetcher));
            resolver.RegisterLazySingleton(() => resolver.GetService<NavigationPagePresenter>(), typeof(IPresenter));
            resolver.RegisterLazySingleton(() => resolver.GetService<NavigationPagePresenter>(), typeof(IPagePresenter));
            //resolver.RegisterLazySingleton(() => new NSCoderObjectStateStore(), typeof(NSCoderObjectStateStore));
            //resolver.RegisterLazySingleton(() => resolver.GetService<NSCoderObjectStateStore>(), typeof(IObjectStateStore));
        }

        public void CloseApp()
        {
        }
    }
}
