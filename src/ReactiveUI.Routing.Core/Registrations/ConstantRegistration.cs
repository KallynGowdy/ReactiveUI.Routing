using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing.Registrations
{
    /// <summary>
    /// Defines a <see cref="IReactiveAppDependency"/> that registers a constant value.
    /// </summary>
    public class ConstantRegistration : IReactiveAppDependency
    {
        public object Constant { get; }
        public Type Registration { get; }
        public string Contract { get; }

        public ConstantRegistration(object constant, Type registration, string contract = null)
        {
            this.Constant = constant;
            this.Registration = registration;
            this.Contract = contract;
        }

        public void Apply(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(Constant, Registration, Contract);
        }
    }
}
