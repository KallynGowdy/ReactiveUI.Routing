using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents an object that can manage the application's navigation stack.
    /// Often, implementations of this interface are combined with a presentation state object to handle navigation and presentation
    /// at the same time.
    /// </summary>
    public interface INavigator : IReActivatable<Unit, NavigationState>
    {
        /// <summary>
        /// Gets the stack of view models that have been transitioned to.
        /// </summary>
        IReadOnlyCollection<Transition> TransitionStack { get; }

        /// <summary>
        /// Gets the observable that represents the asynchronous stream
        /// of transitions.
        /// </summary>
        IObservable<Transition> Transitioned { get; }

        /// <summary>
        /// Adds the given transition to the transition stack.
        /// </summary>
        /// <param name="transition">The transition that should be added.</param>
        /// <returns></returns>
        Task PushAsync(TransitionParams transition);

        /// <summary>
        /// Removes the topmost transition from the transition stack and returns it.
        /// </summary>
        /// <returns></returns>
        Task<Transition> PopAsync();

        /// <summary>
        /// Retrieves the top transition from the transition stack.
        /// Null if no transitions exist on the top stack.
        /// </summary>
        /// <returns></returns>
        Transition Peek();
    }
}
