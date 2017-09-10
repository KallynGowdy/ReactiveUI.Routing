using System;
using System.Reactive;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a presenter that is able to keep track of view states to help perform app suspension.
    /// </summary>
    public interface IAppPresenter : IPresenter
    {
        /// <summary>
        /// Gets the list of currently active views.
        /// </summary>
        ReactiveList<PresentedView> ActiveViews { get; }

        /// <summary>
        /// Gets a new <see cref="AppPresentationState"/> object that represents
        /// a snapshot of the current presentation state.
        /// </summary>
        /// <returns></returns>
        AppPresentationState GetPresentationState();

        /// <summary>
        /// Loads the given presentation state into the app.
        /// </summary>
        /// <param name="state"></param>
        IObservable<Unit> LoadState(AppPresentationState state);
    }
}
