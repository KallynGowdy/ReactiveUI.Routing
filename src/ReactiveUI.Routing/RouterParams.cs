using System;
using System.Collections.Generic;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents the parameters that a router requires.
    /// </summary>
    public sealed class RouterParams
    {
        /// <summary>
        /// Gets or sets the map of view model types to the actions that should be taken on them.
        /// </summary>
        public Dictionary<Type, RouteActions> ViewModelMap { get; set; }
        public object DefaultParameters { get; set; }
        public Type DefaultViewModelType { get; set; }
    }
}