namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a built transition.
    /// </summary>
    public sealed class Transition : ReActivatableObject<ActivationParams, TransitionState>
    {
        /// <summary>
        /// Gets the view model that was created with this transition.
        /// </summary>
        public object ViewModel { get; set; }

        protected override void InitCoreSync(ActivationParams parameters)
        {
            base.InitCoreSync(parameters);
        }
    }
}