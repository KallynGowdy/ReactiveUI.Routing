﻿using System;
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
    public class DefaultDependencies : IRegisterDependencies
    {
        private readonly Assembly[] viewAssemblies;

        public DefaultDependencies(params Assembly[] viewAssemblies)
        {
            this.viewAssemblies = viewAssemblies;
        }

        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            
            resolver.RegisterLazySingleton(() => new Router(), typeof(IRouter));
            resolver.RegisterLazySingleton(() => new Navigator(), typeof(INavigator));
            resolver.Register(() => new DefaultViewTypeLocator(viewAssemblies), typeof(IViewTypeLocator));
        }
    }
}
