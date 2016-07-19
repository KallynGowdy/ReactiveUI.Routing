using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
#pragma warning disable 4014

namespace ReactiveUI.Routing.Tests
{
    public class RouterTests
    {
        private readonly INavigator navigator;
        private readonly IRootPresenter presenter;
        private readonly Router router;

        public RouterTests()
        {
            navigator = Substitute.For<INavigator>();
            presenter = Substitute.For<IRootPresenter>();
            router = new Router(navigator, presenter);
        }

        [Fact]
        public void Test_Ctor_Throws_Exception_When_No_Navigator_Is_Available()
        {
            Assert.Throws<InvalidOperationException>(() => new Router(null, Substitute.For<IRootPresenter>()));
        }

        [Fact]
        public void Test_Ctor_Throws_Exception_When_No_Presenter_Is_Available()
        {
            Assert.Throws<InvalidOperationException>(() => new Router(Substitute.For<INavigator>(), null));
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
        public async Task Test_InitAsync_Pipes_PresenterParams_To_Presenter()
        {
            var parameters = new RouterParams()
            {
                PresenterParams = new RootPresenterParams()
            };
            await router.InitAsync(parameters);
            presenter.Received(1).InitAsync(parameters.PresenterParams);
        }

        [Fact]
        public async Task Test_ShowAsync_Pipes_Transition_To_Navigator_If_Router_Actions_Specify_Navigate()
        {
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
            await router.ShowAsync(typeof(TestViewModel), Unit.Default);

            navigator.Received(1)
                .PushAsync(Arg.Is<ActivationParams>(t =>
                    Equals(t.Params, Unit.Default) &&
                    t.Type == typeof(TestViewModel)));
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
                await router.ShowAsync(typeof(TestViewModel), Unit.Default);
            });
        }
    }
}
#pragma warning restore 4014
