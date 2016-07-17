using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Defines an interface for objects that help build singular routes.
    /// </summary>
    public interface IRouteBuilder
    {
        /// <summary>
        /// Gets the type of the view model that is being configured.
        /// </summary>
        Type ViewModelType { get; }

        /// <summary>
        /// Gets the list of presenters that should be used to present the view model for this route.
        /// </summary>
        IEnumerable<Type> Presenters { get; }

        /// <summary>
        /// Gets the list of actions that should be run against the navigator
        /// when this route is hit.
        /// </summary>
        IEnumerable<Func<INavigator, TransitionParams, Task>> NavigationActions { get; }

        /// <summary>
        /// Sets the type of the view model that this route handles.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        IRouteBuilder SetViewModel(Type viewModelType);

        /// <summary>
        /// Instructs the router to tell the presentation state to use the given
        /// presenter type to display the view model.
        /// </summary>
        /// <param name="presenterType">The type of the presenter.</param>
        /// <returns></returns>
        IRouteBuilder Present(Type presenterType);

        /// <summary>
        /// Instructs the router to navigate backwards while the given predicate function is true.
        /// </summary>
        /// <param name="goBackWhile">A function that, given a parent view model object, returns whether the given view model should be removed from the transition stack.</param>
        /// <returns></returns>
        IRouteBuilder NavigateBackWhile(Func<object, bool> goBackWhile);

        /// <summary>
        /// Instructs the router to transition to the configured view model type.
        /// </summary>
        /// <returns></returns>
        IRouteBuilder Navigate();
    }
}