using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.WPF
{
    public class WpfRoutingDependencies : ReactiveAppBuilder
    {
        public WpfRoutingDependencies()
        {
            this.Register(() => new DefaultWpfSuspensionDriver(), typeof(ISuspensionDriver));
        }
    }
}
