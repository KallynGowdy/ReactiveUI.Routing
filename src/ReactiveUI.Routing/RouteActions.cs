using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a list of actions that the router should perform for a route.
    /// </summary>
    public sealed class RouteActions
    {
        public Func<INavigator, ActivationParams, Task> NavigationAction { get; set; }
    }
}