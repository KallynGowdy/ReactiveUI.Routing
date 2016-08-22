using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Splat;
using UIKit;

namespace ReactiveUI.Routing.iOS
{
    /// <summary>
    /// Defines a <see cref="IPagePresenter"/> that is able to present <see cref="UIViewController"/> in a navigation controller.
    /// </summary>
    public class NavigationControllerPresenter : BasePresenter, IPagePresenter
    {
        private List<UIViewController> controllers = new List<UIViewController>(2);

        private class NavigationController : UINavigationController
        {
            private IRouter Router { get; }

            public NavigationController(UIViewController rootViewController, IRouter router = null) : base(rootViewController)
            {
                Router = router ?? Locator.Current.GetService<IRouter>();
            }

            public override UIViewController PopViewController(bool animated)
            {
                var c = ViewControllers.LastOrDefault();
                Router.BackAsync();
                return c;
            }
        }

        private readonly UIWindow window;
        private NavigationController navigationController;

        public NavigationControllerPresenter(UIWindow window = null, IViewTypeLocator viewLocator = null)
            : base(viewLocator)
        {
            this.window = window ?? Locator.Current.GetService<UIWindow>();
        }

        public override int GetAffinityForView(Type view)
        {
            return typeof(UIViewController).IsAssignableFrom(view) ? 1000 : 0;
        }

        public override Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            if (navigationController == null ||
                navigationController.ViewControllers.All(vc => (vc as IViewFor)?.ViewModel != viewModel))
            {
                var viewModelType = viewModel.GetType();
                var viewType = ResolveViewTypeForViewModelType(viewModelType);
                var view = CreateViewFromType(viewType);
                view.ViewModel = viewModel;
                var viewController = (UIViewController)view;
                return Observable.Start(() =>
                {
                    PushViewController(viewController);
                    NotifyViewActivated(view);
                    return (IDisposable)new ScheduledDisposable(RxApp.MainThreadScheduler,
                        new ActionDisposable(() =>
                        {
                            NotifyViewDeActivated(view);
                            if (!controllers.Remove(viewController)) return;
                            navigationController.SetViewControllers(controllers.ToArray(), true);
                        }));
                }, RxApp.MainThreadScheduler).ToTask();
            }
            else
            {
                return Task.FromResult<IDisposable>(null);
            }
        }

        private void PushViewController(UIViewController viewController)
        {
            controllers.Add(viewController);
            if (window.RootViewController == null)
            {
                InitializeNavigationController(viewController);
            }
            navigationController.SetViewControllers(controllers.ToArray(), true);
        }

        private void InitializeNavigationController(UIViewController view)
        {
            window.RootViewController = navigationController = new NavigationController(view);
            window.MakeKeyAndVisible();
        }
    }
}
