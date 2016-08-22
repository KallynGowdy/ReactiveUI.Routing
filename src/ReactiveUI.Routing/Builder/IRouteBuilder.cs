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
        IRouteBuilder NavigateBackWhile(Func<Transition, bool> goBackWhile);

        /// <summary>
        /// Instructs the router to transition to the configured view model type.
        /// </summary>
        /// <returns></returns>
        IRouteBuilder Navigate();

        /// <summary>
        /// Constructs a new <see cref="RouteActions"/> object from this builder.
        /// </summary>
        /// <returns></returns>
        RouteActions Build();
    }
}