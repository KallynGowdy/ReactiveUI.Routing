using System;
using System.Reactive;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    public class Router : ReActivatableObject<RouterParams, RouterState>, IRouter
    {
        public INavigator Navigator { get; }
        public IRootPresenter Presenter { get; }
        private RouterParams Params { get; set; }

        public Router(): this(null, null)
        {
        }

        public Router(INavigator navigator, IRootPresenter presenter)
        {
            this.Navigator = navigator ?? Locator.Current.GetService<INavigator>();
            this.Presenter = presenter ?? Locator.Current.GetService<IRootPresenter>();
            if (this.Navigator == null) throw new InvalidOperationException("When creating a router, a INavigator object must either be provided or locatable via Locator.Current.GetService<INavigator>()");
            if(this.Presenter == null) throw new InvalidOperationException("When creating a router, a IPresenter object must either be provided or locatable via Locator.Current.GetService<IPresenter>()");
        }

        protected override async Task InitCoreAsync(RouterParams parameters)
        {
            await base.InitCoreAsync(parameters);
            Params = parameters;
            await Navigator.InitAsync(Unit.Default);
            await Presenter.InitAsync(parameters.PresenterParams);
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
            await actions.NavigationAction(Navigator, BuildTransitionParams(viewModel, vmParams));
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
            if(!Initialized) throw new InvalidOperationException("The router must be initialized before use.");
        }
    }
}