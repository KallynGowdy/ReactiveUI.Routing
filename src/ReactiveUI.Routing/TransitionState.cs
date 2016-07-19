namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that stores data for a single transition in the <see cref="NavigationState"/> transition stack.
    /// </summary>
    public sealed class TransitionState
    {
        /// <summary>
        /// Gets or sets the params that should be used to activate the transition.
        /// </summary>
        public ActivationParams Params { get; set; }

        /// <summary>
        /// Gets or sets the state that was saved from the suspended transition.
        /// If null, then the activatable object does not support suspend/resume.
        /// </summary>
        public object State { get; set; }
    }
}