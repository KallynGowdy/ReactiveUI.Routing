using System;
using System.Collections.Generic;
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
            var view = CreateViewFromType(viewType);
            view.ViewModel = viewModel;
            var viewController = (UIViewController)view;
            return Observable.Start(() =>
            {
                PushViewController(viewController);
                NotifyViewActivated(view);
                return (IDisposable)new ScheduledDisposable(
                    RxApp.MainThreadScheduler,
                    new ActionDisposable(() =>
                    {
                        NotifyViewDeActivated(view);
                        navigationController.PopViewController(true);
                    }));
            }, RxApp.MainThreadScheduler).ToTask();
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
