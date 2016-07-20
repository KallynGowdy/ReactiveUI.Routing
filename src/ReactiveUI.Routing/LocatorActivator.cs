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
            return Locator.Current.GetService(parameters.Type);
        }
    }
}
