using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    internal class Registrations : IWantsToRegisterStuff
    {
        public void Register(IMutableDependencyResolver resolver)
        {
            var presenterResolver = new PresenterResolver();
            var appPresenter = new AppPresenter(presenterResolver);
            resolver.RegisterConstant(presenterResolver, typeof(IMutablePresenterResolver));
            resolver.RegisterConstant(presenterResolver, typeof(IPresenterResolver));
            resolver.RegisterConstant(appPresenter, typeof(IAppPresenter));
        }
    }
}
