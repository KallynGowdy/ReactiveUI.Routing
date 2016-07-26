namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that stores data that <see cref="INavigator"/> objects utilize for suspend/resume.
    /// </summary>
    public sealed class NavigatorState
    {
        /// <summary>
        /// Gets or sets the array of <see cref="ObjectState"/> objects that
        /// store the suspended state of each activated object in the stack.
        /// </summary>
        public ObjectState[] TransitionStack { get; set; }
    }
}