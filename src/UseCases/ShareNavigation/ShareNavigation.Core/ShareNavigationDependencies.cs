using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using ShareNavigation.Core;
using ShareNavigation.Core.ViewModels;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Splat;

namespace ShareNavigation
{
    public class ShareNavigationDependencies : IRegisterDependencies
    {
        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            var routerConfig = new RouterBuilder()
                .Default<PhotoListViewModel>()
                .When<PhotoListViewModel>(route => route.Navigate().PresentPage())
                .When<ShareViewModel>(route => route.Navigate().PresentPage())
                .When<PhotoViewModel>(route => route.NavigateFrom<PhotoListViewModel>().PresentPage())
                .Build();
            resolver.RegisterConstant(routerConfig, typeof(RouterConfig));
            resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel));
            resolver.Register(() => new PhotoViewModel(), typeof(PhotoViewModel));
            resolver.Register(() => new ShareViewModel(), typeof(ShareViewModel));
            resolver.Register(() => new PhotosService(), typeof(IPhotosService));
        }
    }
}
