using System;
using System.Runtime.Serialization;

namespace ReactiveUI.Routing.Actions
{
    public class NavigateBackWhileRouteAction : IRouteAction
    {
        [IgnoreDataMember]
        public Func<Transition, bool> GoBackWhile { get; set; }
    }
}