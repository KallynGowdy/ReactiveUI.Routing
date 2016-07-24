using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Defines an interface for objects that help build routers for applications.
    /// The router builders are used primarily to define declarative routes.
    /// </summary>
    public interface IRouterBuilder
    {
        IEnumerable<IRouteBuilder> BuiltRoutes { get; }
        IRouterBuilder When(Func<IRouteBuilder, IRouteBuilder> buildRoute);
        IRouterBuilder Default(Func<IRouteBuilder, IRouteBuilder> buildRoute, object parameters);
        Task<IRouter> BuildAsync();
    }
}
