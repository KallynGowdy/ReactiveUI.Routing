using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public static class RouterExtensions
    {
        /// <summary>
        /// Routes to the given view model type, using the given parameters.
        /// </summary>
        /// <param name="router">The router.</param>
        /// <param name="viewModel">The type of the view model that should be routed to.</param>
        /// <param name="parameters">The parameters that should be passed to the view model.</param>
        /// <returns>Returns a task that represents the async operation.</returns>
        public static Task ShowAsync(this IRouter router, Type viewModel, object parameters)
        {
            return router.DispatchAsync(RouterActions.ShowViewModel(viewModel, parameters));
        }

        /// <summary>
        /// Routes to the given view model type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model that should be routed to.</typeparam>
        /// <param name="router">The router.</param>
        /// <returns>Returns a task that represents the async operation.</returns>
        public static Task ShowAsync<TViewModel>(this IRouter router)
            where TViewModel : IActivatable<Unit>
        {
            return router.ShowAsync<TViewModel, Unit>();
        }

        /// <summary>
        /// Routes to the given view model type, using the given optional parameters.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model that should be routed to.</typeparam>
        /// <typeparam name="TParams">The type of the parameters for the view model.</typeparam>
        /// <param name="router">The router.</param>
        /// <param name="parameters">The parameters that should be passed to the view model. If null, new parameters will be instantiated.</param>
        /// <returns>Returns a task that represents the async operation.</returns>
        public static Task ShowAsync<TViewModel, TParams>(this IRouter router, TParams parameters = default(TParams))
            where TViewModel : IActivatable<TParams>
            where TParams : new()
        {
            if (parameters == null)
            {
                parameters = new TParams();
            }
            return router.ShowAsync(typeof(TViewModel), parameters);
        }

        /// <summary>
        /// Routes to the default view model for the router.
        /// </summary>
        /// <param name="router">The router.</param>
        /// <returns>Returns a task that represents the async operation.</returns>
        public static Task ShowDefaultViewModelAsync(this IRouter router)
        {
            return router.DispatchAsync(RouterActions.ShowDefaultViewModel());
        }

        /// <summary>
        /// Navigates backwards toward the previous view model.
        /// </summary>
        /// <param name="router">The router.</param>
        /// <returns>Returns a task that represents the async operation.</returns>
        public static Task BackAsync(this IRouter router)
        {
            return router.DispatchAsync(RouterActions.Back());
        }
    }
}
