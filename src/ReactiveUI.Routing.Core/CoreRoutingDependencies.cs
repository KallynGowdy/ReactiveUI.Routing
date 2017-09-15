using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an internal class that contains core routing dependencies.
    /// </summary>
    class CoreRoutingDependencies : ReactiveAppBuilder
    {
        public CoreRoutingDependencies()
        {
            var presenterResolver = new PresenterResolver();
            var appPresenter = new AppPresenter(presenterResolver);
            var router = new ReactiveRouter();
            var suspensionDriver = new DefaultSuspensionDriver();

            this.RegisterConstant(presenterResolver, typeof(IMutablePresenterResolver));
            this.RegisterConstant(presenterResolver, typeof(IPresenterResolver));
            this.RegisterConstant(appPresenter, typeof(IAppPresenter));
            this.RegisterConstant(router, typeof(IReactiveRouter));
            this.RegisterConstant(suspensionDriver, typeof(ISuspensionDriver));
        }
    }
}
