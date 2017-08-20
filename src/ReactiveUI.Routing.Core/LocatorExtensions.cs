using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    public static class LocatorExtensions
    {
        public static void RegisterRouting(this IMutableDependencyResolver resolver)
        {
            var presenterResolver = new PresenterResolver();
            var appPresenter = new AppPresenter(presenterResolver);
            resolver.RegisterConstant(presenterResolver, typeof(IMutablePresenterResolver));
            resolver.RegisterConstant(presenterResolver, typeof(IPresenterResolver));
            resolver.RegisterConstant(appPresenter, typeof(IAppPresenter));
        }
    }
}
