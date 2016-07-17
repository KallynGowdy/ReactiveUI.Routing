using System;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for an object that can delegate presentation for different view models to different presenters.
    /// </summary>
    public interface IPresentationState : IPresenter
    {
        /// <summary>
        /// Registers the given view model type to be presented by the given presenter type.
        /// </summary>
        /// <param name="viewModelType">The type of the view model that can be presented.</param>
        /// <param name="presenterType">The type of the presenter that should present the view model.</param>
        void Register(Type viewModelType, Type presenterType);

        /// <summary>
        /// Removes the given view model and presenter type pair from this object's list of registrations.
        /// </summary>
        /// <param name="viewModelType">The type of the view model.</param>
        /// <param name="presenterType">The type of the presenter.</param>
        void UnRegister(Type viewModelType, Type presenterType);
    }
}