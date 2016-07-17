namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a built transition.
    /// </summary>
    public sealed class Transition : ReActivatableObject<TransitionParams, TransitionState>
    {
        /// <summary>
        /// Gets the view model that was created with this transition.
        /// </summary>
        public object ViewModel { get; set; }

        protected override void InitCoreSync(TransitionParams parameters)
        {
            base.InitCoreSync(parameters);
        }
    }
}