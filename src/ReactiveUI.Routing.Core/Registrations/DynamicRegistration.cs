using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing.Registrations
{
    /// <summary>
    /// Defines a class that is able to dynamically register dependencies based on whether a given type
    /// that implements <see cref="IReactiveAppDependency"/> is available.
    /// </summary>
    public class DynamicRegistration : IReactiveAppDependency
    {
        /// <summary>
        /// Gets the assembly qualified type name that should be checked.
        /// </summary>
        public string TypeName { get; }

        public DynamicRegistration(string typeName)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        }

        public void Apply(IMutableDependencyResolver resolver)
        {
            Type type = Type.GetType(TypeName);
            if (type != null)
            {
                var dependency = (IReactiveAppDependency)Activator.CreateInstance(type);
                dependency.Apply(resolver);
            }
        }
    }
}
