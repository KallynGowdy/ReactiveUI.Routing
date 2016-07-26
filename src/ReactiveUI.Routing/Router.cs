using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a typical router.
    /// </summary>
    public class Router : ReActivatableObject<RouterParams, RouterState>, IRouter
    {
        private INavigator Navigator { get; }
        private IActivator Activator { get; }
        private RouterParams Params { get; set; }

        public Router() : this(null, null)
        {
        }

        public Router(INavigator navigator) : this(navigator, null)
        {
        }

        public Router(INavigator navigator, IActivator activator)
        {
            this.Navigator = navigator ?? Locator.Current.GetService<INavigator>();
            this.Activator = activator ?? Locator.Current.GetService<IActivator>() ?? new LocatorActivator();
            if (this.Navigator == null) throw new InvalidOperationException($"When creating a router, a {nameof(INavigator)} object must either be provided or locatable via Locator.Current.GetService<{nameof(INavigator)}>()");
        }

        protected override async Task InitCoreAsync(RouterParams parameters)
        {
            await base.InitCoreAsync(parameters);
            Params = parameters;
            await Navigator.InitAsync(Unit.Default);
            if (Params.DefaultViewModelType != null && Params.DefaultParameters != null)
            {
                await ShowCoreAsync(Params.DefaultViewModelType, Params.DefaultParameters);
            }
        }

        protected override async Task ResumeCoreAsync(RouterState storedState, IReActivator reActivator)
        {
            await base.ResumeCoreAsync(storedState, reActivator);
            await Navigator.ResumeAsync(storedState.NavigatorState, reActivator);
        }

        protected override async Task<RouterState> SuspendCoreAsync()
        {
            var state = await base.SuspendCoreAsync();
            state.NavigatorState = await Navigator.SuspendAsync();
            return state;
        }

        protected override async Task DestroyCoreAsync()
        {
            await base.DestroyCoreAsync();
            await Navigator.DestroyAsync();
        }

        protected async Task ShowCoreAsync(Type viewModel, object vmParams)
        {
            var actions = GetActionsForViewModelType(viewModel);
            var transition = await BuildTransitionAsync(viewModel, vmParams);
            await Navigate(actions, transition);
            await PresentAsync(actions, transition);
        }

        public async Task ShowAsync(Type viewModel, object vmParams)
        {
            CheckInit();
            await ShowCoreAsync(viewModel, vmParams);
        }

        public Task HideAsync(object viewModel)
        {
            throw new NotImplementedException();
        }

        private async Task Navigate(RouteActions actions, Transition transition)
        {
            if (actions.NavigationAction != null)
            {
                await actions.NavigationAction(Navigator, transition);
            }
        }

        private async Task PresentAsync(RouteActions actions, Transition transition)
        {
            if (actions.Presenters == null) return;
            foreach (var presenter in actions.Presenters)
            {
                await PresentAsync(presenter, transition);
            }
        }

        private async Task<IDisposable> PresentAsync(Type presenter, Transition transition)
        {
            var p = await BuildPresenterAsync(presenter);
            return await p.PresentAsync(transition.ViewModel, null);
        }

        private async Task<IPresenter> BuildPresenterAsync(Type presenter)
        {
            return (IPresenter)await Activator.ActivateAsync(new ActivationParams()
            {
                Type = presenter,
                Params = Unit.Default
            });
        }

        private async Task<Transition> BuildTransitionAsync(Type viewModel, object vmParams)
        {
            var activationParams = BuildTransitionParams(viewModel, vmParams);
            var transition = new Transition();
            await transition.InitAsync(activationParams);
            return transition;
        }

        private ActivationParams BuildTransitionParams(Type viewModel, object vmParams)
        {
            return new ActivationParams()
            {
                Type = viewModel,
                Params = vmParams
            };
        }

        protected virtual RouteActions GetActionsForViewModelType(Type viewModel)
        {
            RouteActions actions;
            if (Params.ViewModelMap.TryGetValue(viewModel, out actions))
            {
                return actions;
            }
            else
            {
                throw new InvalidOperationException($"Cannot navigate to type {viewModel}. It was not found in the view model map");
            }
        }

        private void CheckInit()
        {
            if (!Initialized) throw new InvalidOperationException("The router must be initialized before use.");
        }

        public static async Task<IRouter> InitWithParamsAsync(RouterParams routerParams)
        {
            var router = new Router();
            await router.InitAsync(routerParams);
            return router;
        }
    }
}