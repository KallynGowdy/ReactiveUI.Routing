﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Extensions for <see cref="IRouterBuilder"/>.
    /// </summary>
    public static class RouterBuilderExtensions
    {
        public static IRouterBuilder When<TViewModel>(this IRouterBuilder builder,
            Func<IRouteBuilder, IRouteBuilder> buildRoute) =>
                builder.When(route => buildRoute(route.SetViewModel(typeof(TViewModel))));

        public static IRouterBuilder When<TViewModel>(this IRouterBuilder builder) =>
            builder.When(route => route.SetViewModel(typeof(TViewModel)));
    }
}
