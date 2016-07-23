using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using ShareNavigation.Presenters;
using ShareNavigation.ViewModels;

namespace ShareNavigation
{
    public class RouterConfig
    {
        public async Task<IRouter> BuildRouterAsync(INavigator navigator = null)
        {
            var builder = navigator != null ? new RouterBuilder(() => navigator) : new RouterBuilder();
            builder
                .When<PhotoListViewModel>(route => route.Navigate().Present<IPhotoListPresenter>())
                .When<ShareViewModel>(route => route.Navigate().Present<ISharePresenter>())
                .When<PhotoViewModel>(route => route.NavigateFrom<PhotoListViewModel>().Navigate().Present<IPhotoPresenter>());
            return await builder.BuildAsync();
        }
    }
}
