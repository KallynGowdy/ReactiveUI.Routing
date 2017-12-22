using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace ReactiveUI.Routing
{
    internal interface IWantsToRegisterStuff
    {
        void Register(IMutableDependencyResolver resolver);
    }
}
