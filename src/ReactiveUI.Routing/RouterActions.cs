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

        public static IRouterAction Back()
        {
            return new NavigateBackAction();
        }
    }
}