using System;
using System.Collections.Generic;
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
        private readonly UIWindow window;
        private UINavigationController navigationController;

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
            var viewModelType = viewModel.GetType();
            var viewType = ResolveViewTypeForViewModelType(viewModelType);
            var view = (IViewFor)Locator.Current.GetService(viewType);
            view.ViewModel = viewModel;
            var viewController = (UIViewController)view;
            PushViewController(viewController);
            NotifyViewActivated(view);

            return Task.FromResult<IDisposable>(new ActionDisposable(() =>
            {
                NotifyViewDeActivated(view);
                navigationController.PopViewController(true);
            }));
        }

        private void PushViewController(UIViewController viewController)
        {
            if (window.RootViewController == null)
            {
                InitializeNavigationController(viewController);
            }
            else
            {
                navigationController.PushViewController(viewController, true);
            }
        }

        private void InitializeNavigationController(UIViewController view)
        {
            window.RootViewController = navigationController = new UINavigationController(view);
            window.MakeKeyAndVisible();
        }
    }
}
