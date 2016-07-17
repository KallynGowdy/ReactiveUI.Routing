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
        public static IRouteBuilder Present<TPresenter>(this IRouteBuilder route) =>
            route.Present(typeof(TPresenter));

        public static IRouteBuilder NavigateFrom<TParentViewModel>(this IRouteBuilder route) =>
            route.NavigateBackWhile(vm => !(vm is TParentViewModel)).Navigate();
    }
}
