using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public class Navigator : ReActivatableObject<Unit, NavigatorState>, INavigator
    {
        private List<Transition> Transitions { get; } = new List<Transition>();
        private BehaviorSubject<Transition> Subject { get; } = new BehaviorSubject<Transition>(null);
        public IReadOnlyCollection<Transition> TransitionStack => Transitions;
        public IObservable<Transition> OnTransition => Subject;

        private void Notify()
        {
            Subject.OnNext(Peek());
        }

        public Task PushAsync(Transition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));
            Transitions.Add(transition);
            Notify();
            return Task.FromResult(0);
        }

        public Task<Transition> PopAsync()
        {
            if (Transitions.Count <= 0) throw new InvalidOperationException("Cannot pop transition from stack. There are currently no transitions to pop.");
            var trans = Peek();
            Transitions.RemoveAt(Transitions.Count - 1);
            Notify();
            return Task.FromResult(trans);
        }

        public Transition Peek()
        {
            return Transitions.LastOrDefault();
        }

        protected override async Task<NavigatorState> SuspendCoreAsync()
        {
            var state = await base.SuspendCoreAsync();
            state.TransitionStack = await Task.WhenAll(Transitions.Select(t => t.SuspendAsync()).ToArray());
            return state;
        }

        protected override async Task ResumeCoreAsync(NavigatorState storedState, IReActivator reActivator)
        {
            await base.ResumeCoreAsync(storedState, reActivator);
            if (storedState.TransitionStack != null)
            {
                foreach (var transition in storedState.TransitionStack)
                {
                    var resumed = (Transition)await reActivator.ResumeAsync(transition);
                    await PushAsync(resumed);
                }
            }
        }
    }
}