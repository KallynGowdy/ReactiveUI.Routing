using System;

namespace ReactiveUI.Routing.Actions
{
    public class NavigateBackWhileRouteAction : IRouteAction
    {
        public Func<Transition, bool> GoBackWhile { get; set; }
    }
}