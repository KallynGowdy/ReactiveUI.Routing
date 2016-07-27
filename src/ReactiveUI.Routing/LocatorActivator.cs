using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents an <see cref="IActivator"/> that uses the current locator to instantiate objects.
    /// </summary>
    public class LocatorActivator : BaseActivator
    {
        protected override object InstantiateObject(ActivationParams parameters)
        {
            var obj = Locator.Current.GetService(parameters.Type);
            if(obj == null) throw new InvalidOperationException($"Could not resolve an object of type: {parameters.Type} because Locator.Current.GetService({parameters.Type}) returned null. Make sure you have registered a factory for the type using Locator.CurrentMutable.Register(factory, {parameters.Type})");
            return obj;
        }
    }
}
