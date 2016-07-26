using System;
using System.Linq;
using System.Text;
using ReactiveUI.Routing.Builder;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a default configuration for routed applications.
    /// </summary>
    public abstract class DefaultRoutedAppConfig : IRoutedAppConfig
    {
        public virtual void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            resolver.Register(() => new DefaultViewTypeLocator(), typeof(IViewTypeLocator));
            resolver.Register(() => new Navigator(), typeof(INavigator));
            resolver.Register(() => new LocatorActivator(), typeof(IActivator));
            resolver.Register(() => new Router(), typeof(IRouter));
            resolver.Register(BuildRouterParamsSafe, typeof(RouterParams));
        }

        private RouterParams BuildRouterParamsSafe()
        {
            var parameters = BuildRouterParams();
            if (parameters == null)
            {
                throw new InvalidReturnValueException($"{nameof(BuildRouterParams)} must return a non-null value, as it is a required dependency registration.");
            }
            return parameters;
        }

        protected abstract RouterParams BuildRouterParams();
    }
}
