using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents a router. That is, an object
    /// that controls the selection of logic when showing a view model.
    /// </summary>
    public interface IRouter : IReActivatable<RouterConfig, RouterState>
    {
        /// <summary>
        /// Dispatches the given action to the router.
        /// </summary>
        /// <param name="action">The action that the router should perform.</param>
        /// <returns></returns>
        Task DispatchAsync(IRouterAction action);
    }
}
