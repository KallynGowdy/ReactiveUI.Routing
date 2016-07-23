﻿using System;
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
        public IEnumerable<IRouteBuilder> BuiltRoutes => builtRoutes;

        public RouterBuilder()
        {
        }

        public RouterBuilder(Func<INavigator> navigatorFactory)
        {
            this.navigatorFactory = navigatorFactory;
        }

        public IRouterBuilder When(Func<IRouteBuilder, IRouteBuilder> buildRoute)
        {
            if (buildRoute == null) throw new ArgumentNullException(nameof(buildRoute));
            var routeBuilder = new RouteBuilder();
            var built = buildRoute(routeBuilder);
            builtRoutes.Add(built);
            return this;
        }

        public async Task<IRouter> BuildAsync()
        {
            INavigator navigator = null;
            if (navigatorFactory != null)
            {
                navigator = navigatorFactory();
            }
            var router = new Router(navigator);

            return router;
        }
    }
}
