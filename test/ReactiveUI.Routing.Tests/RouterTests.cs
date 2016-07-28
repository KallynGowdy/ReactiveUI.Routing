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
using Splat;
using Xunit;
#pragma warning disable 4014

namespace ReactiveUI.Routing.Tests
{
    public class RouterTests : LocatorTest
    {
        private readonly INavigator navigator;
        private readonly Router router;

        public RouterTests()
        {
            navigator = Substitute.For<INavigator>();
            router = new Router(navigator);
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
                await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), null));
            });
        }

        [Fact]
        public async Task Test_InitAsync_Initializes_Navigator()
        {
            await router.InitAsync(new RouterParams());
            navigator.Received(1).InitAsync(Unit.Default);
        }

        [Fact]
        public async Task Test_ResumeAsync_Resumes_Navigator()
        {
            var state = new RouterState()
            {
                NavigatorState = new NavigatorState()
            };
            await router.ResumeAsync(state, Substitute.For<IReActivator>());
            navigator.Received(1).ResumeAsync(state.NavigatorState, Arg.Any<IReActivator>());
        }

        [Fact]
        public async Task Test_SuspendAsync_Suspends_Navigator()
        {
            var navigationState = new NavigatorState();
            navigator.SuspendAsync().Returns(navigationState);

            var state = await router.SuspendAsync();

            state.NavigatorState.Should().Be(navigationState);
        }

        [Fact]
        public async Task Test_DestroyAsync_Destroys_Navigator()
        {
            await router.DestroyAsync();
            navigator.Received(1).DestroyAsync();
        }

        [Fact]
        public async Task Test_ShowAsync_Pipes_Transition_To_Navigator_If_Router_Actions_Specify_Navigate()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = (nav, p) => nav.PushAsync(p)
                        }
                    }
                }
            };

            await router.InitAsync(initParams);
            await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));

            navigator.Received(1)
                .PushAsync(Arg.Is<Transition>(t => t.ViewModel is TestViewModel));
        }

        [Fact]
        public async Task Test_ShowAsync_Does_Not_Pipe_Transition_To_Navigator_If_Router_Actions_Specify_Navigate()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = null
                        }
                    }
                }
            };

            await router.InitAsync(initParams);
            await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));

            navigator.DidNotReceive()
                .PushAsync(Arg.Any<Transition>());
        }

        [Fact]
        public async Task Test_ShowAsync_Throws_If_Given_Type_Is_Not_In_Map()
        {
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
            };

            await router.InitAsync(initParams);
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));
            });
        }

        [Fact]
        public async Task Test_ShowAsync_Creates_Presenter_If_Router_Actions_Specify_Present()
        {
            Func<TestPresenterType> presenterConstructor = Substitute.For<Func<TestPresenterType>>();
            presenterConstructor().Returns(new TestPresenterType());
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(presenterConstructor, typeof(TestPresenterType));
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Presenters = new [] { typeof(TestPresenterType) }
                        }
                    }
                }
            };

            await router.InitAsync(initParams);
            await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));

            presenterConstructor.Received(1)();
        }

        [Fact]
        public async Task Test_ShowAsync_Calls_PresentAsync_On_Created_Presenter()
        {
            IPresenter presenter = Substitute.For<IPresenter>();
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(() => presenter, typeof(TestPresenterType));
            var subject = new Subject<Transition>();
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Presenters = new [] { typeof(TestPresenterType) }
                        }
                    }
                }
            };
            navigator.PushAsync(Arg.Any<Transition>()).Returns(ci =>
            {
                subject.OnNext(ci.Arg<Transition>());
                return Task.FromResult(0);
            });

            await router.InitAsync(initParams);
            await router.DispatchAsync(RouterActions.ShowViewModel(typeof(TestViewModel), new TestParams()));

            presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>());
        }

        [Fact]
        public async Task Test_InitAsync_Navigates_To_DefaultViewModel()
        {
            Resolver.Register(() => new TestViewModel(), typeof(TestViewModel));
            var action = Substitute.For<Func<INavigator, Transition, Task>>();
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = action
                        }
                    }
                },
                DefaultViewModelType = typeof(TestViewModel),
                DefaultParameters = new TestParams()
            };

            await router.InitAsync(initParams);

            action.Received(1)(navigator, Arg.Is<Transition>(t => t.ViewModel is TestViewModel));
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_Navigator_State()
        {
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = (nav, transition) => nav.PushAsync(transition)
                        }
                    }
                }
            };

            var state = new NavigatorState();
            navigator.SuspendAsync().Returns(state);
            await router.InitAsync(initParams);
            var stored = await router.SuspendAsync();

            stored.NavigatorState.Should().Be(state);
        }

        [Fact]
        public async Task Test_Router_Presents_Transition_Resolved_From_OnTransition()
        {
            IPresenter presenter = Substitute.For<IPresenter>();
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            Locator.CurrentMutable.Register(() => presenter, typeof(TestPresenterType));
            var subject = new Subject<TransitionEvent>();
            var viewModel = new TestViewModel();
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = (nav, transition) => nav.PushAsync(transition),
                            Presenters = new Type[]
                            {
                                typeof(TestPresenterType)
                            }
                        }
                    }
                }
            };
            navigator.OnTransition.Returns(subject);
            await router.InitAsync(initParams);

            subject.OnNext(new TransitionEvent()
            {
                Current = new Transition()
                {
                    ViewModel = viewModel
                }
            });

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
            var subject = new Subject<TransitionEvent>();
            var viewModel = new TestViewModel();
            var trans = new Transition()
            {
                ViewModel = viewModel
            };
            var initParams = new RouterParams()
            {
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            NavigationAction = (nav, transition) => nav.PushAsync(transition),
                            Presenters = new Type[]
                            {
                                typeof(TestPresenterType)
                            }
                        }
                    }
                }
            };
            navigator.OnTransition.Returns(subject);
            await router.InitAsync(initParams);

            subject.OnNext(new TransitionEvent()
            {
                Current = trans
            });

            disposable.IsDisposed.Should().BeFalse();

            subject.OnNext(new TransitionEvent()
            {
                Current = new Transition()
                {
                    ViewModel = new TestViewModel()
                },
                Previous = trans
            });

            disposable.IsDisposed.Should().BeTrue();
        }
    }
}
#pragma warning restore 4014
