using System;
using System.Reactive;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a reactive application
    /// </summary>
    public interface IReactiveApp : IDisposable
    {
        /// <summary>
        /// Gets the router that can be used to handle
        /// linear navigation in the app.
        /// </summary>
        IReactiveRouter Router { get; }

        /// <summary>
        /// Gets the presenter for the app.
        /// </summary>
        IAppPresenter Presenter { get; }

        /// <summary>
        /// Gets the configured suspension host.
        /// </summary>
        ISuspensionHost SuspensionHost { get; }

        /// <summary>
        /// Gets the configured suspension driver.
        /// </summary>
        ISuspensionDriver SuspensionDriver { get; }

        /// <summary>
        /// Gets the configured dependency resolver.
        /// </summary>
        IMutableDependencyResolver Locator { get; }

        /// <summary>
        /// Gets the current application state.
        /// </summary>
        /// <returns></returns>
        ReactiveAppState BuildAppState();

        /// <summary>
        /// Loads the given state into the application.
        /// </summary>
        /// <param name="state">The state that should be loaded into the application.</param>
        IObservable<Unit> LoadState(ReactiveAppState state);

        /// <summary>
        /// Registers the given disposable to be disposed once the application closes.
        /// </summary>
        /// <param name="disposable">The object that should be disposed.</param>
        void RegisterDisposable(IDisposable disposable);
    }
}