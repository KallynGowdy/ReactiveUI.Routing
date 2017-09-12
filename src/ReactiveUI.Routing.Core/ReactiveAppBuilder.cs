using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a reactive application builder.
    /// That is, an object which helps compile a list of dependencies needed to get an application running.
    /// </summary>
    public class ReactiveAppBuilder : IReactiveAppBuilder
    {
        private readonly List<IReactiveAppDependency> dependencies = new List<IReactiveAppDependency>();

        public void Apply(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            foreach (var dependency in dependencies)
            {
                dependency.Apply(resolver);
            }
        }

        public IReactiveAppBuilder Add(IReactiveAppDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));
            dependencies.Add(dependency);

            return this;
        }
    }
}
