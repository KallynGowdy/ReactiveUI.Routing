using System;
using System.Collections.Generic;
using System.Linq;
using Splat;

namespace ReactiveUI.Routing
{
    public class DependencyResolverProxy : IMutableDependencyResolver
    {
        private readonly IMutableDependencyResolver resolver;

        public HashSet<Type> RegisteredServices { get; } = new HashSet<Type>();

        public DependencyResolverProxy(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            this.resolver = resolver;
        }

        public void Dispose() => resolver.Dispose();
        public object GetService(Type serviceType, string contract = null) => resolver.GetService(serviceType, contract);

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
            => resolver.GetServices(serviceType, contract);

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            resolver.Register(factory, serviceType, contract);
            RegisteredServices.Add(serviceType);
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
            => resolver.ServiceRegistrationCallback(serviceType, contract, callback);
    }

    /// <summary>
    /// Defines a config that asserts that the configuration is in a valid state.
    /// </summary>
    public class AppConfig : CompositeRoutedAppConfig
    {
        public AppConfig(params IRegisterDependencies[] configs) : base(configs)
        {
        }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            var proxy = new DependencyResolverProxy(resolver);
            base.RegisterDependencies(proxy);
            AssertContainsTypes(proxy, 
                typeof(RouterConfig),
                typeof(IRouter),
                typeof(IObjectStateStore),
                typeof(ISuspensionNotifier));
        }

        private void AssertContainsTypes(DependencyResolverProxy proxy, params Type[] types)
        {
            var missingTypes = new List<Type>(types.Length);
            missingTypes.AddRange(types.Where(t => !proxy.RegisteredServices.Contains(t)));
            if (missingTypes.Count <= 0) return;
            var missingTypesStr = String.Join(", ", missingTypes.Select(t => t.ToString()).OrderBy(s => s));
            throw new InvalidOperationException("Application is missing required registrations: " + missingTypesStr);
        }
    }
}