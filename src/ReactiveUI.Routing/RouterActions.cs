using System;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents various actions that a router can take.
    /// These actions represent things like "Show View Model", or "Go Back".
    /// </summary>
    public static class RouterActions
    {
        /// <summary>
        /// Creates a new <see cref="ShowViewModelAction"/>, which instructs the router to route to the given view model type.
        /// </summary>
        /// <param name="viewModel">The view model that should be routed to.</param>
        /// <param name="parameters">The parameters that should be passed to the view model.</param>
        /// <returns>A new <see cref="ShowViewModelAction"/>.</returns>
        public static ShowViewModelAction ShowViewModel(Type viewModel, object parameters)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            return new ShowViewModelAction()
            {
                ActivationParams = new ActivationParams()
                {
                    Type = viewModel,
                    Params = parameters
                }
            };
        }

        /// <summary>
        /// Creates a new <see cref="NavigateBackAction"/>, which instructs the router to navigate backwards.
        /// </summary>
        /// <param name="closeAppIfNeeded"></param>
        /// <returns></returns>
        public static NavigateBackAction Back(bool closeAppIfNeeded)
        {
            return new NavigateBackAction(closeAppIfNeeded);
        }

        /// <summary>
        /// Creates a new <see cref="ShowDefaultViewModelAction"/>, which instructs the router to route to the default view model.
        /// </summary>
        /// <returns></returns>
        public static ShowDefaultViewModelAction ShowDefaultViewModel()
        {
            return new ShowDefaultViewModelAction();
        }
    }
}