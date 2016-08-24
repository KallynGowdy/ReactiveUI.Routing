using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;
using ReactiveUI.Routing.Builder;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a typical router.
    /// </summary>
    public class Router : ReActivatableObject<RouterConfig, RouterState>, IRouter
    {
        public class StoredRouterAction
        {
            public IRouterAction Action { get; set; }
            public object ViewModelState { get; set; }
        }

        private class ActiveRouterAction
        {
            public IRouterAction Action { get; set; }
            public Transition Transition { get; set; }
        }

        private readonly Subject<Unit> closeApp = new Subject<Unit>();
        private INavigator Navigator { get; }
        private List<ActiveRouterAction> Actions { get; } = new List<ActiveRouterAction>();
        private Dictionary<Transition, List<IDisposable>> Presenters { get; } = new Dictionary<Transition, List<IDisposable>>();

        public override bool SaveInitParams => false;
        public IObservable<Unit> CloseApp => closeApp;

        public Router() : this(null)
        {
        }

        public Router(INavigator navigator)
        {
            this.Navigator = navigator ?? Locator.Current.GetService<INavigator>();
            if (this.Navigator == null) throw new InvalidOperationException($"When creating a router, a {nameof(INavigator)} object must either be provided or locatable via Locator.Current.GetService<{nameof(INavigator)}>()");
        }

        protected override async Task ResumeCoreAsync(RouterState storedState)
        {
            await base.ResumeCoreAsync(storedState);
            foreach (var action in storedState.Actions)
            {
                await DispatchAsync(action);
            }
        }

        private async Task DispatchAsync(StoredRouterAction action)
        {
            // TODO: Cleanup to represent unidirectional dataflow
            // Rework to manipulate state through reducers and then
            // present based on the current state.

            await When<ShowViewModelAction>(action.Action, async showViewModelAction =>
            {
                await ShowViewModelAsync(showViewModelAction, action.ViewModelState);
            });
            await When<NavigateBackAction>(action.Action, async navigateBackAction =>
            {
                await NavigateBackAsync(navigateBackAction);
            });
            await When<ShowDefaultViewModelAction>(action.Action, async a =>
            {
                if (Actions.Count == 0 && InitParams?.DefaultViewModelType != null && InitParams.DefaultParameters != null)
                {
                    var stored = new StoredRouterAction()
                    {
                        Action = RouterActions.ShowViewModel(InitParams.DefaultViewModelType, InitParams.DefaultParameters),
                        ViewModelState = action.ViewModelState
                    };
                    await DispatchAsync(stored);
                }
            });
        }

        protected override async Task<RouterState> GetStateCoreAsync()
        {
            var state = await base.GetStateCoreAsync();
            List<StoredRouterAction> outputActions = new List<StoredRouterAction>();
            foreach (var action in Actions)
            {
                object vmState = null;
                var vm = action.Transition.ViewModel as IReActivatable;
                if (vm != null)
                {
                    vmState = await vm.GetStateAsync();
                }
                outputActions.Add(new StoredRouterAction()
                {
                    Action = action.Action,
                    ViewModelState = vmState
                });
            }
            state.Actions = outputActions.ToArray();
            return state;
        }

        private async Task ShowViewModelAsync(ShowViewModelAction action)
        {
            await ShowViewModelAsync(action, null);
        }

        private async Task ShowViewModelAsync(ShowViewModelAction action, object state)
        {
            var transition = await BuildViewModelAsync(action.ActivationParams);
            var reActivatable = transition.ViewModel as IReActivatable;
            if (reActivatable != null && state != null)
            {
                await reActivatable.ResumeAsync(state);
            }
            Actions.Add(new ActiveRouterAction()
            {
                Action = action,
                Transition = transition
            });
            var routeActions = GetActionsForViewModelType(action.ActivationParams.Type);
            await HandleRouteActionsAsync(routeActions, transition);
        }

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
                if (Actions.Count == 0 && InitParams?.DefaultViewModelType != null && InitParams.DefaultParameters != null)
                {
                    await
                        DispatchAsync(RouterActions.ShowViewModel(InitParams.DefaultViewModelType, InitParams.DefaultParameters));
                }
            });
        }

        private async Task NavigateBackAsync(NavigateBackAction navigateBackAction)
        {
            Actions.RemoveAt(Actions.Count - 1);
            var transition = await Navigator.PopAsync();
            await DisposeTransitionAsync(transition);
            var current = Navigator.Peek();
            if (current?.ViewModel != null)
            {
                var actions = GetActionsForViewModelType(current.ViewModel.GetType());
                await HandleRoutePresentation(actions.Actions, current);
            }
            else
            {
                closeApp.OnNext(Unit.Default);
            }
        }

        private async Task DisposeTransitionAsync(Transition transition)
        {
            if (transition != null)
            {
                DisposePresenters(transition);
                await ActivationHelpers.DestroyObjectAsync(transition.ViewModel);
            }
        }

        private void DisposePresenters(Transition removed)
        {
            List<IDisposable> disposables;
            if (Presenters.TryGetValue(removed, out disposables))
            {
                foreach (var d in disposables)
                {
                    d.Dispose();
                }
                Presenters.Remove(removed);
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
            if (actions.Actions != null && !Presenters.ContainsKey(transition))
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
                    await this.BackAsync();
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
            if (d != null)
            {
                AddDisposable(transition, d);
            }
        }

        private void AddDisposable(Transition transition, IDisposable disposable)
        {
            List<IDisposable> list;
            if (Presenters.TryGetValue(transition, out list))
            {
                list.Add(disposable);
            }
            else
            {
                Presenters.Add(transition, new List<IDisposable> { disposable });
            }
        }

        private async Task<IPresenter> BuildPresenterAsync(Type presenter)
        {
            return (IPresenter)await ActivationHelpers.CreateAndInitObjectAsync(new ActivationParams()
            {
                Type = presenter,
                Params = Unit.Default
            });
        }

        private async Task<Transition> BuildViewModelAsync(ActivationParams activationParams)
        {
            var vm = await ActivationHelpers.CreateAndInitObjectAsync(activationParams);
            var transition = new Transition()
            {
                ViewModel = vm
            };
            return transition;
        }

        protected virtual RouteActions GetActionsForViewModelType(Type viewModel)
        {
            RouteActions actions;
            if (InitParams.ViewModelMap.TryGetValue(viewModel, out actions))
            {
                return actions;
            }
            else
            {
                throw new InvalidOperationException($"Cannot navigate to type {viewModel}. It was not found in the view model map. Make sure you are calling {nameof(RouterBuilder.When)} on your {nameof(RouterBuilder)}.");
            }
        }

        private void CheckInit()
        {
            if (!Initialized) throw new InvalidOperationException("The router must be initialized before use.");
        }
    }
}