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
        /// Loads the given presentation state into the app.
        /// </summary>
        /// <param name="state"></param>
        IObservable<Unit> LoadState(AppPresentationState state);
    }
}
