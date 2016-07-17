namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that stores data that <see cref="INavigator"/> objects utilize for suspend/resume.
    /// </summary>
    public sealed class NavigationState
    {
        /// <summary>
        /// Gets or sets the array of <see cref="TransitionState"/> objects that
        /// store the suspended state of each activated object in the stack.
        /// </summary>
        public TransitionState[] TransitionStack { get; set; }
    }
}