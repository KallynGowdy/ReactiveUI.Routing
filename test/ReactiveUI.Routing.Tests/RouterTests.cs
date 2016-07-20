using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
                await router.ShowAsync(typeof(TestViewModel), null);
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
                NavigationState = new NavigationState()
            };
            await router.ResumeAsync(state);
            navigator.Received(1).ResumeAsync(state.NavigationState);
        }

        [Fact]
        public async Task Test_SuspendAsync_Suspends_Navigator()
        {
            var navigationState = new NavigationState();
            navigator.SuspendAsync().Returns(navigationState);

            var state = await router.SuspendAsync();

            state.NavigationState.Should().Be(navigationState);
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
            await router.ShowAsync(typeof(TestViewModel), new TestParams());

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
            await router.ShowAsync(typeof(TestViewModel), new TestParams());

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
                await router.ShowAsync(typeof(TestViewModel), new TestParams());
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
            await router.ShowAsync(typeof(TestViewModel), new TestParams());

            presenterConstructor.Received(1)();
        }
    }
}
#pragma warning restore 4014
