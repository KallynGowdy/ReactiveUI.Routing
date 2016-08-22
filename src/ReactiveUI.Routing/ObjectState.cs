namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that stores data for an object that was suspended.
    /// </summary>
    public sealed class ObjectState
    {
        /// <summary>
        /// Gets or sets the params that should be used to activate the object.
        /// </summary>
        public ActivationParams Params { get; set; }

        /// <summary>
        /// Gets or sets the state that was saved from the suspended object.
        /// If null, then the activatable object does not support suspend/resume.
        /// </summary>
        public object State { get; set; }
    }
}