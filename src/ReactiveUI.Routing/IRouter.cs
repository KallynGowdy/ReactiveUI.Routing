using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents a router. That is, an object
    /// that controls the selection of logic when showing a view model.
    /// </summary>
    public interface IRouter : IReActivatable<RouterParams, RouterState>
    {
        /// <summary>
        /// Attempts to display the given view model type.
        /// </summary>
        /// <param name="viewModel">The type of the view model that should be displayed.</param>
        /// <param name="vmParams">The parameters that should be passed to the view model during setup.</param>
        /// <returns></returns>
        Task ShowAsync(Type viewModel, object vmParams);

        /// <summary>
        /// Attempts to destruct and hide the given view model.
        /// </summary>
        /// <param name="viewModel">The object that is currently being shown.</param>
        /// <returns></returns>
        Task HideAsync(object viewModel);
    }
}
