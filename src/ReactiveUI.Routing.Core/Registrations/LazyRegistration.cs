using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveUI.Routing.Registrations
{
    /// <summary>
    /// Defines a factory that can create <see cref="IReactiveAppDependency"/> that resolve to the internal lazy singleton.
    /// </summary>
    public class LazyRegistration
    {
        public object Result { get; private set; }
        private Func<object> InternalFactory { get; }

        public LazyRegistration(Func<object> factory)
        {
            InternalFactory = factory;
        }

        public IReactiveAppDependency CreateDependency(Type registration, string contract = null)
        {
            return new FactoryRegistration(GetResult, registration, contract);
        }

        private object GetResult()
        {
            return Result ?? (Result = InternalFactory());
        }
    }
}
