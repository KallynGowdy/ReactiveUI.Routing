using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Extensions for <see cref="IRouteBuilder"/>.
    /// </summary>
    public static class RouteBuilderExtensions
    {
        /// <summary>
        /// Instructs the builder to present this route with a default <see cref="IPresenter"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static IRouteBuilder Present(this IRouteBuilder route) => route.Present<IPresenter>();

        /// <summary>
        /// Instructs the builder to present this route with a <see cref="IPagePresenter"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static IRouteBuilder PresentPage(this IRouteBuilder route) => route.Present<IPagePresenter>();

        /// <summary>
        /// Instructs the builder to present this route with a <see cref="IToastPresenter"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static IRouteBuilder PresentToast(this IRouteBuilder route) => route.Present<IToastPresenter>();

        /// <summary>
        /// Instructs the route builder to present this route with the given <typeparamref name="TPresenter"/> type.
        /// </summary>
        /// <typeparam name="TPresenter">The type of presenter that should be used to present the route.</typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public static IRouteBuilder Present<TPresenter>(this IRouteBuilder route) where TPresenter : IPresenter =>
            route.Present(typeof(TPresenter));

        /// <summary>
        /// Instructs the route builder that the given <typeparamref name="TParentViewModel"/> type should
        /// be the hierarchal parent of this route, such that when navigating backwards, the parent view model type is displayed.
        /// </summary>
        /// <typeparam name="TParentViewModel">The type of view model that should be directly above this route.</typeparam>
        /// <param name="route"></param>
        /// <returns></returns>
        public static IRouteBuilder NavigateFrom<TParentViewModel>(this IRouteBuilder route) =>
            route.NavigateBackWhile(vm => !(vm.ViewModel is TParentViewModel)).Navigate();
    }
}
