using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Splat;

namespace ShareNavigation
{
    public class RoutedAppConfig : DefaultRoutedAppConfig
    {
        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel));
            resolver.Register(() => new PhotoViewModel(), typeof(PhotoViewModel));
            resolver.Register(() => new ShareViewModel(), typeof(ShareViewModel));
            resolver.Register(() => new PhotosService(), typeof(IPhotosService));
            resolver.RegisterConstant(new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }, typeof(JsonSerializerSettings));
        }

        protected override RouterParams BuildRouterParams()
        {
            return new RouterBuilder()
                .Default<PhotoListViewModel>()
                .When<PhotoListViewModel>(route => route.Navigate().Present())
                .When<ShareViewModel>(route => route.Navigate().Present())
                .When<PhotoViewModel>(route => route.NavigateFrom<PhotoListViewModel>().Present())
                .Build();
        }

        protected override ISuspensionNotifier BuildSuspensionNotifier()
        {
            throw new NotImplementedException();
        }

        protected override IObjectStateStore BuildObjectStateStore()
        {
            return new AkavacheObjectStateStore();
        }
    }
}
