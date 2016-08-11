using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Actions
{
    /// <summary>
    /// Defines an action that instructs the router to show a view model with parameters.
    /// </summary>
    public sealed class ShowViewModelAction : IRouterAction
    {
        /// <summary>
        /// The parameters that should be used to activate the view model.
        /// </summary>
        public ActivationParams ActivationParams { get; set; }


    }
}
