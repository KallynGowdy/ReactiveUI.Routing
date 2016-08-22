using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Defines a class that implements a basic <see cref="IRouterBuilder"/>.
    /// </summary>
    public class RouterBuilder : IRouterBuilder
    {
        private readonly Func<INavigator> navigatorFactory;
        private readonly List<IRouteBuilder> builtRoutes = new List<IRouteBuilder>();
        private IRouteBuilder DefaultRoute { get; set; }
        private object DefaultParameters { get; set; }
        public IEnumerable<IRouteBuilder> BuiltRoutes => builtRoutes;

        public RouterBuilder()
        {
        }

        public RouterBuilder(Func<INavigator> navigatorFactory)
        {
            this.navigatorFactory = navigatorFactory;
        }

        protected IRouteBuilder BuildRoute(Func<IRouteBuilder, IRouteBuilder> buildRoute)
        {
            if (buildRoute == null) throw new ArgumentNullException(nameof(buildRoute));
            var routeBuilder = new RouteBuilder();
            return buildRoute(routeBuilder);
        }

        public IRouterBuilder When(Func<IRouteBuilder, IRouteBuilder> buildRoute)
        {
            var built = BuildRoute(buildRoute);
            builtRoutes.Add(built);
            return this;
        }

        public IRouterBuilder Default(Func<IRouteBuilder, IRouteBuilder> buildRoute, object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var built = BuildRoute(buildRoute);
            DefaultRoute = built;
            DefaultParameters = parameters;
            return this;
        }

        public RouterConfig Build()
        {
            return new RouterConfig()
            {
                ViewModelMap = BuiltRoutes.Select(r => r.Build()).ToDictionary(a => a.ViewModelType),
                DefaultViewModelType = DefaultRoute?.ViewModelType,
                DefaultParameters = DefaultParameters
            };
        }
    }
}
