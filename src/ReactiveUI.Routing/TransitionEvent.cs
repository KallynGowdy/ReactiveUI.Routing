using System;

namespace ReactiveUI.Routing
{
    public class TransitionEvent
    {
        public Transition Current { get; set; }
        public Transition Removed { get; set; }
    }
}