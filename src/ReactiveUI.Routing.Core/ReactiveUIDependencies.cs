using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing
{
    internal class ReactiveUIDependencies : IReactiveAppDependency
    {
        public void Apply(IMutableDependencyResolver resolver)
        {
            resolver.InitializeReactiveUI();
            resolver.Register(() => RxApp.SuspensionHost ?? (RxApp.SuspensionHost = new DefaultSuspensionHost()), typeof(ISuspensionHost));
        }
    }
}
