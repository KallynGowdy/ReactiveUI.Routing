using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.UWP
{
    public class UwpRoutingDependencies : ReactiveAppBuilder
    {
        public UwpRoutingDependencies()
        {
            this.Register(() => new UwpPageActivationForViewFetcher(), typeof(IActivationForViewFetcher));
        }
    }
}
