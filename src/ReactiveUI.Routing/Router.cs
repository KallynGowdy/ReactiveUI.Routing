using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a typical router.
    /// </summary>
    public class Router : ReActivatableObject<RouterParams, RouterState>, IRouter
    {
        public class StoredRouterAction
        {
            public IRouterAction Action { get; set; }
            public Transition Transition { get; set; }
        }

        private readonly ObservableAsPropertyHelper<RouterParams> parameters;
        private INavigator Navigator { get; }
        private IActivator Activator { get; }
        private RouterParams Params => parameters.Value;
        private List<StoredRouterAction> Actions { get; } = new List<StoredRouterAction>();
        private readonly Dictionary<Transition, List<IDisposable>> presenters = new Dictionary<Transition, List<IDisposable>>();

        public override bool SaveInitParams => false;

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

            parameters = this.OnActivated.FirstAsync().ToProperty(this, r => r.Params);
        }

        private void DisposePresenters(Transition removed)
        {
            if (removed != null)
            {
                List<IDisposable> disposables;
                if (presenters.TryGetValue(removed, out disposables))
                {
                    foreach (var d in disposables)
                    {
                        d.Dispose();
                    }
                    presenters.Remove(removed);
                }
            }
        }

        protected override async Task ResumeCoreAsync(RouterState storedState, IReActivator reActivator)
        {
            await base.ResumeCoreAsync(storedState, reActivator);
            foreach (var action in storedState.Actions)
            {
                await DispatchAsync(action.Action, action.Transition);
            }
        }

        private async Task DispatchAsync(IRouterAction action, Transition transition)
        {
            await When<ShowViewModelAction>(action, async showViewModelAction =>
            {
                await ShowViewModelAsync(showViewModelAction, transition);
            });
            await When<NavigateBackAction>(action, async navigateBackAction =>
            {
                await NavigateBackAsync(navigateBackAction);
            });
            await When<ShowDefaultViewModelAction>(action, async a =>
            {
                if (Actions.Count == 0 && Params?.DefaultViewModelType != null && Params.DefaultParameters != null)
                {
                    await DispatchAsync(RouterActions.ShowViewModel(Params.DefaultViewModelType, Params.DefaultParameters), transition);
                }
            });
        }

        protected override async Task<RouterState> SuspendCoreAsync()
        {
            var state = await base.SuspendCoreAsync();
            state.Actions = Actions.ToArray();
            return state;
        }

        private async Task ShowViewModelAsync(ShowViewModelAction action)
        {
            var activationParams = action.ActivationParams;
            var transition = await BuildTransitionAsync(activationParams);
            await ShowViewModelAsync(action, transition);
        }

        private async Task ShowViewModelAsync(ShowViewModelAction action, Transition transition)
        {
            Actions.Add(new StoredRouterAction()
            {
                Action = action,
                Transition = transition
            });
            var routeActions = GetActionsForViewModelType(action.ActivationParams.Type);
            await HandleRouteActionsAsync(routeActions, transition);
        }

        // Show 1 -> Show 2 -> Back (Show 1)
        // 
        // Show 1:
        // Action is dispatched, transition is created, view model is presented.
        // Show 2:
        // Action is dispatched, transition created and view model is presented.
        // Back:
        // Transition is popped, Show 2 presenters are disposed, Show 1 is presented.

        public async Task DispatchAsync(IRouterAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            CheckInit();
            await When<ShowViewModelAction>(action, async showViewModelAction =>
            {
                await ShowViewModelAsync(showViewModelAction);
            });
            await When<NavigateBackAction>(action, async navigateBackAction =>
            {
                await NavigateBackAsync(navigateBackAction);
            });
            await When<ShowDefaultViewModelAction>(action, async a =>
            {
                if (Actions.Count == 0 && Params?.DefaultViewModelType != null && Params.DefaultParameters != null)
                {
                    await
                        DispatchAsync(RouterActions.ShowViewModel(Params.DefaultViewModelType, Params.DefaultParameters));
                }
            });
        }

        private async Task NavigateBackAsync(NavigateBackAction navigateBackAction)
        {
            Actions.RemoveAt(Actions.Count - 1);
            var transition = await Navigator.PopAsync();
            DisposePresenters(transition);
            var current = Navigator.Peek();
            if (current != null)
            {
                var actions = GetActionsForViewModelType(current.ViewModel.GetType());
                await HandleRoutePresentation(actions.Actions, current);
            }
        }

        private async Task HandleRoutePresentation(IRouteAction[] actions, Transition transition)
        {
            foreach (var action in actions)
            {
                await HandleRoutePresentation(action, transition);
            }
        }

        protected async Task When<T>(IRouterAction action, Func<T, Task> operation)
            where T : class, IRouterAction
        {
            var convertedAction = action as T;
            if (convertedAction != null)
            {
                await operation(convertedAction);
            }
        }

        protected async Task WhenAction<T>(IRouteAction action, Func<T, Task> operation)
            where T : class, IRouteAction
        {
            var convertedAction = action as T;
            if (convertedAction != null)
            {
                await operation(convertedAction);
            }
        }

        private async Task HandleRouteActionsAsync(RouteActions actions, Transition transition)
        {
            if (actions.Actions != null && !presenters.ContainsKey(transition))
            {
                foreach (var action in actions.Actions)
                {
                    await HandleRouteActionAsync(action, transition);
                }
            }
        }

        private async Task HandleRouteNavigationAsync(IRouteAction action, Transition transition)
        {
            await WhenAction<NavigateRouteAction>(action, async n =>
            {
                if (Navigator.Peek() != transition)
                {
                    await Navigator.PushAsync(transition);
                }
            });
            await WhenAction<NavigateBackWhileRouteAction>(action, async n =>
            {
                while (Navigator.TransitionStack.Count > 0 && n.GoBackWhile(Navigator.Peek()))
                {
                    await Navigator.PopAsync();
                }
            });
        }

        private async Task HandleRouteActionAsync(IRouteAction action, Transition transition)
        {
            await HandleRouteNavigationAsync(action, transition);
            await HandleRoutePresentation(action, transition);
        }

        private async Task HandleRoutePresentation(IRouteAction action, Transition transition)
        {
            await WhenAction<PresentRouteAction>(action, async p => { await PresentAsync(p, transition); });
        }

        private async Task PresentAsync(PresentRouteAction presenter, Transition transition)
        {
            var p = await BuildPresenterAsync(presenter.PresenterType);
            var d = await p.PresentAsync(transition.ViewModel, presenter.Hint);
            AddDisposable(transition, d);
        }

        private void AddDisposable(Transition transition, IDisposable disposable)
        {
            List<IDisposable> list;
            if (presenters.TryGetValue(transition, out list))
            {
                list.Add(disposable);
            }
            else
            {
                presenters.Add(transition, new List<IDisposable> { disposable });
            }
        }

        private async Task<IPresenter> BuildPresenterAsync(Type presenter)
        {
            return (IPresenter)await Activator.ActivateAsync(new ActivationParams()
            {
                Type = presenter,
                Params = Unit.Default
            });
        }

        private async Task<Transition> BuildTransitionAsync(ActivationParams activationParams)
        {
            var vm = await Activator.ActivateAsync(activationParams);
            if (vm == null) throw new InvalidOperationException($"Cannot initialize transition. The returned value from the Locator.Current.GetService({activationParams.Type}) was null, which means that the view model could not be instantiated.");
            var transition = new Transition()
            {
                ViewModel = vm
            };
            return transition;
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