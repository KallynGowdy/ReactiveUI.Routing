using System;

namespace ReactiveUI.Routing.Actions
{
    /// <summary>
    /// Defines a <see cref="IRouterAction"/> that instructs the router to present a view model.
    /// </summary>
    public class PresentAction : IRouterAction
    {
        public Type PresenterType { get; set; }
        public object Hint { get; set; }
    }
}