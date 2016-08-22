using System;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a built transition.
    /// </summary>
    public sealed class Transition
    {
        /// <summary>
        /// Gets the view model that was created with this transition.
        /// </summary>
        public object ViewModel { get; set; }

        public override string ToString()
        {
            var type = ViewModel?.GetType()?.ToString() ?? "NULL";
            return $@"Transition to: {type}";
        }
    }
}