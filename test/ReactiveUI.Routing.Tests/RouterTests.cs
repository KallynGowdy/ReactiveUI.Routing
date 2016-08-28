using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Actions;
using ReactiveUI.Routing.Builder;
using Splat;
using Xunit;
#pragma warning disable 4014

namespace ReactiveUI.Routing.Tests
{
    public class RouterTests : LocatorTest
    {
        public INavigator Navigator { get; private set; }
        public Router Router { get; private set; }

        public RouterTests()
        {
            Navigator = Substitute.For<INavigator>();
            Router = new Router(Navigator);
        }

        [Fact]
        public void Test_Ctor_Throws_Exception_When_No_Navigator_Is_Available()
        {
            Assert.Throws<InvalidOperationException>(() => new Router(null));
        }

        [Fact]
        public async Task Test_ShowAsync_Throws_If_Router_Is_Not_Initialized()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));
            });
        }

        [Fact]
        public async Task Test_ShowAsync_Works_With_Unit_Params()
        {
            Locator.CurrentMutable.Register(() => new UnitViewModel(), typeof(UnitViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(UnitViewModel),
                        new RouteActions()
                        {
                            Actions = new [] { RouteActions.Navigate() }
                        }
                    }
                }
            };

            await Router.InitAsync(initParams);
            await Router.ShowAsync<UnitViewModel>();

            Navigator.Received(1)
                .PushAsync(Arg.Is<Transition>(t => t.ViewModel is UnitViewModel));
        }

        [Fact]
        public async Task Test_ShowAsync_Pipes_Transition_To_Navigator_If_Router_Actions_Specify_Navigate()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new [] { RouteActions.Navigate() }
                        }
                    }
                }
            };

            await Router.InitAsync(initParams);
            await Router.ShowAsync(typeof(TestViewModel), new TestParams());

            Navigator.Received(1)
                .PushAsync(Arg.Is<Transition>(t => t.ViewModel is TestViewModel));
        }

        [Fact]
        public async Task Test_ShowAsync_Does_Not_Pipe_Transition_To_Navigator_If_Router_Actions_Specify_Navigate()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                    }
                }
            };

            await Router.InitAsync(initParams);
            await Router.ShowAsync(typeof(TestViewModel), new TestParams());

            Navigator.DidNotReceive()
                .PushAsync(Arg.Any<Transition>());
        }

        [Fact]
        public async Task Test_ShowAsync_Throws_If_Given_Type_Is_Not_In_Map()
        {
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
            };

            await Router.InitAsync(initParams);
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Router.ShowAsync(typeof(TestViewModel), new TestParams());
            });
        }

        [Fact]
        public async Task Test_ShowAsync_Creates_Presenter_If_Router_Actions_Specify_Present()
        {
            Func<TestPresenterType> presenterConstructor = Substitute.For<Func<TestPresenterType>>();
            presenterConstructor().Returns(new TestPresenterType());
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(presenterConstructor, typeof(TestPresenterType));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Present(typeof(TestPresenterType))
                            }
                        }
                    }
                }
            };

            await Router.InitAsync(initParams);
            await Router.ShowAsync(typeof(TestViewModel), new TestParams());

            presenterConstructor.Received(1)();
        }

        [Fact]
        public async Task Test_ShowAsync_Calls_PresentAsync_On_Created_Presenter()
        {
            IPresenter presenter = Substitute.For<IPresenter>();
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(() => presenter, typeof(TestPresenterType));
            var subject = new Subject<Transition>();
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Present(typeof(TestPresenterType))
                            }
                        }
                    }
                }
            };
            Navigator.PushAsync(Arg.Any<Transition>()).Returns(ci =>
            {
                subject.OnNext(ci.Arg<Transition>());
                return Task.FromResult(0);
            });

            await Router.InitAsync(initParams);
            await Router.ShowAsync(typeof(TestViewModel), new TestParams());

            presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>());
        }

        [Fact]
        public async Task Test_Router_Presents_Transition_Resolved_From_OnTransition()
        {
            var viewModel = new TestViewModel();
            var subject = new Subject<TransitionEvent>();
            IPresenter presenter = Substitute.For<IPresenter>();
            Locator.CurrentMutable.Register(() => presenter, typeof(TestPresenterType));
            Locator.CurrentMutable.Register(() => viewModel, typeof(TestViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Navigate(),
                                RouteActions.Present(typeof(TestPresenterType))
                            }
                        }
                    }
                }
            };
            Navigator.OnTransition.Returns(subject);
            await Router.InitAsync(initParams);

            Router.ShowAsync<TestViewModel, TestParams>();

            presenter.Received(1).PresentAsync(viewModel, null);
        }

        [Fact]
        public async Task Test_Router_Disposes_Of_Presenters_After_Transition()
        {
            IPresenter presenter = Substitute.For<IPresenter>();
            var disposable = new BooleanDisposable();
            presenter.PresentAsync(Arg.Any<object>(), Arg.Any<object>()).Returns(disposable);
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(() => presenter, typeof(TestPresenterType));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Navigate(),
                                RouteActions.Present(typeof(TestPresenterType))
                            }
                        }
                    }
                }
            };
            Transition trans = null;
            Navigator.PushAsync(Arg.Any<Transition>()).Returns(c =>
            {
                trans = c.Arg<Transition>();
                return Task.FromResult(0);
            });
            Navigator.PopAsync().Returns(c => trans);
            await Router.InitAsync(initParams);

            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.ShowAsync<TestViewModel, TestParams>();

            disposable.IsDisposed.Should().BeFalse();

            await Router.BackAsync();
            disposable.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task Test_NavigateBackAction_Causes_Router_To_Navigate_Backwards()
        {
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Navigate()
                            }
                        }
                    }
                }
            };

            await Router.InitAsync(initParams);
            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.BackAsync();

            Navigator.Received(1).PopAsync();
        }

        [Fact]
        public async Task Test_NavigateBackWhileAction_Causes_Rotuer_To_Navigate_Backwards_While_The_Func_Is_True()
        {
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterConfig()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new[]
                            {
                                RouteActions.NavigateBackWhile(transition => transition.ViewModel is TestViewModel),
                                RouteActions.Navigate()
                            }
                        }
                    }
                }
            };
            Navigator.TransitionStack.Count.Returns(1);
            Navigator.Peek().Returns(new Transition()
            {
                ViewModel = new TestViewModel()
            }, new Transition()
            {

            });
            await Router.InitAsync(initParams);
            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.ShowAsync<TestViewModel, TestParams>();

            Navigator.Received(1).PopAsync();
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_RouterState()
        {
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestViewModel, TestParams>();

            var state = await Router.GetStateAsync();

            Assert.Collection(state.Actions,
                a =>
                {
                    a.Action.Should().BeAssignableTo<ShowViewModelAction>();
                    var action = a.Action.As<ShowViewModelAction>();
                    action.ActivationParams.Params.Should().BeAssignableTo<TestParams>();
                    action.ActivationParams.Type.Should().Be<TestViewModel>();
                    a.ViewModelState.Should().BeNull();
                });
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_RouterState_With_ViewModel_State()
        {
            var viewModel = new TestReActivatableViewModel()
            {
                State = new TestState()
                {
                    Value = "Hello, State!"
                }
            };
            Resolver.RegisterConstant(viewModel, typeof(TestReActivatableViewModel));
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestReActivatableViewModel>(r => r.Navigate())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestReActivatableViewModel, TestParams>();

            var state = await Router.GetStateAsync();
            Assert.Collection(state.Actions,
                a =>
                {
                    a.Action.Should().BeAssignableTo<ShowViewModelAction>();
                    var action = a.Action.As<ShowViewModelAction>();
                    action.ActivationParams.Params.Should().BeAssignableTo<TestParams>();
                    action.ActivationParams.Type.Should().Be<TestReActivatableViewModel>();
                    a.ViewModelState.Should().BeAssignableTo<TestState>();
                    a.ViewModelState.As<TestState>().Value.Should().Be("Hello, State!");
                });
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_RouterState_With_ViewModel_Params()
        {
            var viewModel = new TestReActivatableViewModel();
            Resolver.RegisterConstant(viewModel, typeof(TestReActivatableViewModel));
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestReActivatableViewModel>(r => r.Navigate())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestReActivatableViewModel, TestParams>(new TestParams()
            {
                Value = "Hello, Params!"
            });

            var state = await Router.GetStateAsync();
            Assert.Collection(state.Actions,
                a =>
                {
                    a.Action.Should().BeAssignableTo<ShowViewModelAction>();
                    var action = a.Action.As<ShowViewModelAction>();
                    action.ActivationParams.Params.Should().BeAssignableTo<TestParams>();
                    action.ActivationParams.Params.As<TestParams>().Value.Should().Be("Hello, Params!");
                });
        }

        [Fact]
        public async Task Test_ResumeAsync_Dispatches_Saved_Actions()
        {
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate())
                .Build();
            var savedState = new RouterState()
            {
                Actions = new[]
                {
                    new Router.StoredRouterAction()
                    {
                        Action = RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()),
                        ViewModelState = null
                    },
                }
            };

            await Router.InitAsync(routerParams);
            await Router.ResumeAsync(savedState);

            Assert.Collection(Navigator.TransitionStack,
                t => t.ViewModel.Should().BeAssignableTo<TestViewModel>());
        }

        [Fact]
        public async Task Test_ResumeAsync_Resumes_Stored_ViewModel_State()
        {
            Resolver.Register(() => new TestReActivatableViewModel(), typeof(TestReActivatableViewModel));
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestReActivatableViewModel>(r => r.Navigate())
                .Build();
            var viewModelState = new TestState();
            var savedState = new RouterState()
            {
                Actions = new[]
                {
                    new Router.StoredRouterAction()
                    {
                        Action = RouterActions.ShowViewModel(typeof(TestReActivatableViewModel), new TestParams()),
                        ViewModelState = viewModelState
                    },
                }
            };

            await Router.InitAsync(routerParams);
            await Router.ResumeAsync(savedState);

            Assert.Collection(Navigator.TransitionStack,
                t =>
                {
                    t.ViewModel.Should().BeAssignableTo<TestReActivatableViewModel>();
                    t.ViewModel.As<TestReActivatableViewModel>().State.Should().Be(viewModelState);
                });
        }

        [Fact]
        public async Task Test_NavigateBackAction_Presents_Previous_ViewModel()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var presenter = Substitute.For<IPresenter>();
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            Resolver.RegisterConstant(presenter, typeof(IPresenter));
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate().Present())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.BackAsync();

            presenter.Received(3).PresentAsync(Arg.Any<object>(), Arg.Any<object>());
        }

        [Fact]
        public async Task Test_InvalidOperationException_Is_Thrown_If_ViewModel_Cannot_Be_Located()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate().Present())
                .Build();

            await Router.InitAsync(routerParams);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Router.ShowAsync<TestViewModel, TestParams>();
            });
        }

        [Fact]
        public async Task
            Test_InvalidOperationException_Is_Thrown_If_Attempting_To_Navigate_To_ViewModel_That_Is_Not_In_RouterParams()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var routerParams = new RouterBuilder()
                .Build();

            await Router.InitAsync(routerParams);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Router.ShowAsync<TestViewModel, TestParams>();
            });
        }

        [Fact]
        public async Task Test_CloseApp_Resolves_When_Navigating_Back_From_Root_ViewModel()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.ShowAsync<TestViewModel, TestParams>();

            List<Unit> closeNotifications = new List<Unit>();
            Router.CloseApp.Subscribe(u => closeNotifications.Add(u));

            await Router.BackAsync();
            closeNotifications.Should().BeEmpty();
            await Router.BackAsync();
            Assert.Collection(closeNotifications,
                u => u.Should().NotBeNull());
        }

        [Fact]
        public async Task Test_NavigateBackAction_Destroys_ViewModel()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var viewModel = new TestViewModel();
            Resolver.RegisterConstant(viewModel, typeof(TestViewModel));
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate())
                .Build();

            await Router.InitAsync(routerParams);
            await Router.ShowAsync<TestViewModel, TestParams>();

            viewModel.Destroyed.Should().BeFalse();

            await Router.BackAsync();

            viewModel.Destroyed.Should().BeTrue();
        }

        [Fact]
        public async Task Test_NavigateBackWhile_Disposes_Of_Presenters_For_ViewModel()
        {
            Navigator = new Navigator();
            Router = new Router(Navigator);
            var testViewModel = new TestViewModel();
            var middleViewModel = new MiddleViewModel();
            var otherViewModel = new OtherViewModel();
            var middlePresenter = Substitute.For<IPresenter>();
            var disposable = new BooleanDisposable();
            Resolver.RegisterConstant(testViewModel, typeof(TestViewModel));
            Resolver.RegisterConstant(middleViewModel, typeof(MiddleViewModel));
            Resolver.RegisterConstant(otherViewModel, typeof(OtherViewModel));
            Resolver.RegisterConstant(middlePresenter, typeof(IPresenter));
            var routerParams = new RouterBuilder()
                .When<TestViewModel>(r => r.Navigate())
                .When<MiddleViewModel>(r => r.Navigate().Present<IPresenter>())
                .When<OtherViewModel>(r => r.NavigateFrom<TestViewModel>())
                .Build();
            middlePresenter.PresentAsync(middleViewModel, null).Returns(disposable);

            await Router.InitAsync(routerParams);

            await Router.ShowAsync<TestViewModel, TestParams>();
            await Router.ShowAsync<MiddleViewModel, TestParams>();

            disposable.IsDisposed.Should().BeFalse();

            await Router.ShowAsync<OtherViewModel, TestParams>();
            
            disposable.IsDisposed.Should().BeTrue();
        }
    }
}
#pragma warning restore 4014
