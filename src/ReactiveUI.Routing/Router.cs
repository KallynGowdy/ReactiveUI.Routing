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
            if (this.Navigator == null) throw new InvalidOperationException("When creating a router, a INavigator object must either be provided or locatable via Locator.Current.GetService<INavigator>()");
        }

        protected override async Task InitCoreAsync(RouterParams parameters)
        {
            await base.InitCoreAsync(parameters);
            Params = parameters;
            await Navigator.InitAsync(Unit.Default);
        }

        protected override async Task ResumeCoreAsync(RouterState storedState)
        {
            await base.ResumeCoreAsync(storedState);
            await Navigator.ResumeAsync(storedState.NavigationState);
        }

        protected override async Task<RouterState> SuspendCoreAsync()
        {
            var state = await base.SuspendCoreAsync();
            state.NavigationState = await Navigator.SuspendAsync();
            return state;
        }

        protected override async Task DestroyCoreAsync()
        {
            await base.DestroyCoreAsync();
            await Navigator.DestroyAsync();
        }

        public async Task ShowAsync(Type viewModel, object vmParams)
        {
            CheckInit();
            var actions = GetActionsForViewModelType(viewModel);
            var transition = await BuildTransitionAsync(viewModel, vmParams);
            await Navigate(actions, transition);
            await PresentAsync(actions, transition);
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
    }
}