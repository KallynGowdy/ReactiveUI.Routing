using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Builder;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a default configuration for routed applications.
    /// </summary>
    public class DefaultRoutedAppConfig : IRoutedAppConfig
    {
        public virtual void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.Register(() => new DefaultViewTypeLocator(), typeof(IViewTypeLocator));
            resolver.Register(() => new LocatorActivator(), typeof(IActivator));
        }

        public virtual async Task<IRouter> BuildRouterAsync()
        {
            var builder = new RouterBuilder();
            var routerParams = builder.Build();
            return await Router.InitWithParamsAsync(routerParams);
        }
    }
}
