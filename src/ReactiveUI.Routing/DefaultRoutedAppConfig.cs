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
        public virtual void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            
            resolver.RegisterConstant(new LocatorActivator(), typeof(IActivator));
            resolver.RegisterLazySingleton(BuildSuspensionNotifier, typeof(ISuspensionNotifier));
            resolver.RegisterLazySingleton(BuildObjectStateStore, typeof(IObjectStateStore));
            resolver.RegisterLazySingleton(() => new Router(), typeof(IRouter));
            resolver.RegisterLazySingleton(BuildRouterParamsSafe, typeof(RouterConfig));
            resolver.RegisterLazySingleton(() => new Navigator(), typeof(INavigator));
            resolver.RegisterLazySingleton(() => ReActivator.Current, typeof(IReActivator));
            resolver.Register(() => new DefaultViewTypeLocator(GetType().GetTypeInfo().Assembly), typeof(IViewTypeLocator));
        }


        private RouterConfig BuildRouterParamsSafe()
        {
            var parameters = BuildRouterParams();
            if (parameters == null)
            {
                throw new InvalidReturnValueException($"{nameof(BuildRouterParams)} must return a non-null value, as it is a required dependency registration.");
            }
            return parameters;
        }

        public abstract void CloseApp();
        protected abstract RouterConfig BuildRouterParams();
        protected abstract ISuspensionNotifier BuildSuspensionNotifier();
        protected abstract IObjectStateStore BuildObjectStateStore();
    }
}
