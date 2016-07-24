using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using ShareNavigation.Presenters;
using ShareNavigation.ViewModels;
using Splat;

namespace ShareNavigation
{
    public class RoutedAppConfig : DefaultRoutedAppConfig
    {
        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
        }

        public override async Task<IRouter> BuildRouterAsync()
        {
            var builder = new RouterBuilder();
            return await builder
                .When<PhotoListViewModel>(route => route.Navigate().Present<IPhotoListPresenter>())
                .When<ShareViewModel>(route => route.Navigate().Present<ISharePresenter>())
                .When<PhotoViewModel>(
                    route => route.NavigateFrom<PhotoListViewModel>().Navigate().Present<IPhotoPresenter>())
                .BuildAsync();
        }
    }
}
