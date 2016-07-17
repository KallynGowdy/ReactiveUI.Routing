using System;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents parameters that are needed to activate an object.
    /// </summary>
    public sealed class TransitionParams
    {
        /// <summary>
        /// Gets or sets the type of object that this transition represents.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the parameters that should be passed to the activated object.
        /// </summary>
        public object Params { get; set; }
    }
}