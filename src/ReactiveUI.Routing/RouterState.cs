using System.Collections.Generic;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing
{
    public sealed class RouterState
    {
        /// <summary>
        /// Gets or sets the list of actions that were performed on the router.
        /// </summary>
        public Router.StoredRouterAction[] Actions { get; set; }
    }
}