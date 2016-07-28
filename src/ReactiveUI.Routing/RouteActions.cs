using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a list of actions that the router should perform for a route.
    /// </summary>
    public sealed class RouteActions
    {
        /// <summary>
        /// Gets or sets the array of actions that the router should perform.
        /// </summary>
        [Obsolete("Not implemented")]
        public IRouterAction[] Actions { get; set; }
        /// <summary>
        /// Gets or sets the navigation action that should be run when the route is hit.
        /// </summary>
        public Func<INavigator, Transition, Task> NavigationAction { get; set; }
        /// <summary>
        /// Gets or sets the array of presenter types that should be used to present the view model.
        /// </summary>
        public Type[] Presenters { get; set; }
        public Type ViewModelType { get; set; }
    }
}