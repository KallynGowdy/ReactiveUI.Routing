using System;
using System.Linq;
using System.Reflection;
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
        private readonly Lazy<IRouter> Router = new Lazy<IRouter>(() => new Router());

        public virtual void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            resolver.RegisterLazySingleton(BuildSuspensionNotifier, typeof(ISuspensionNotifier));
            resolver.RegisterLazySingleton(BuildObjectStateStore, typeof(IObjectStateStore));
            resolver.Register(() => new DefaultViewTypeLocator(GetType().GetTypeInfo().Assembly), typeof(IViewTypeLocator));
            resolver.Register(() => new Navigator(), typeof(INavigator));
            resolver.Register(() => new LocatorActivator(), typeof(IActivator));
            resolver.Register(() => new ReActivator(), typeof(IReActivator));
            resolver.Register(() => Router.Value, typeof(IRouter));
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
        protected abstract ISuspensionNotifier BuildSuspensionNotifier();
        protected abstract IObjectStateStore BuildObjectStateStore();
    }
}
