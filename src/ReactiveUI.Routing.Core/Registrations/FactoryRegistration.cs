using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing.Registrations
{
    /// <summary>
    /// Defines a <see cref="IReactiveAppDependency"/> that uses the given factory to resolve dependencies.
    /// </summary>
    public class FactoryRegistration : IReactiveAppDependency
    {
        public Func<object> Factory { get; protected set; }
        public Type Registration { get; }
        public string Contract { get; }

        public FactoryRegistration(Func<object> factory, Type registration, string contract = null)
        {
            Factory = factory;
            Registration = registration;
            Contract = contract;
        }

        protected FactoryRegistration(Type registration, string contract = null)
        {
            Registration = registration;
            Contract = contract;
        }

        public void Apply(IMutableDependencyResolver resolver)
        {
            resolver.Register(Factory, Registration, Contract);
        }
    }
}
