using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    public class ReActivator : BaseReActivator
    {
        public ReActivator(IActivator activator = null) : base(activator ?? Locator.Current.GetService<IActivator>() ?? new LocatorActivator())
        {
        }
    }
}
