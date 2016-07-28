using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a list of actions that the router should perform for a route.
    /// </summary>
    public sealed class RouteActions
    {
        /// <summary>
        /// Gets or sets the array of actions that the router should perform.
        /// </summary>
        public IRouteAction[] Actions { get; set; }

        /// <summary>
        /// Gets or sets the type of the view model that these actions are for.
        /// </summary>
        public Type ViewModelType { get; set; }

        public static PresentRouteAction Present(Type presenterType, object hint = null)
        {
            if (presenterType == null) throw new ArgumentNullException(nameof(presenterType));
            return new PresentRouteAction()
            {
                PresenterType = presenterType,
                Hint = hint
            };
        }

        public static NavigateRouteAction Navigate()
        {
            return new NavigateRouteAction();
        }

        public static IRouteAction NavigateBackWhile(Func<Transition, bool> goBackWhile)
        {
            return new NavigateBackWhileRouteAction()
            {
                GoBackWhile = goBackWhile
            };
        }
    }
}