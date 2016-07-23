using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Builder;
using Xunit;
#pragma warning disable 4014

namespace ReactiveUI.Routing.Tests
{
    public class TestViewModel : ActivatableObject<TestParams>
    {
        public new bool Initialized => base.Initialized;
    }

    public class TestPresenterType : IPresenter
    {
        public Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            return Task.FromResult<IDisposable>(new BooleanDisposable());
        }
    }

    public class RouteBuilderTests
    {
        public RouteBuilder Route { get; }

        public RouteBuilderTests()
        {
            Route = new RouteBuilder();
            Route.NavigationActions.Should().BeEmpty();
            Route.Presenters.Should().BeEmpty();
            Route.ViewModelType.Should().BeNull();
        }

        [Fact]
        public void Test_SetViewModel_Sets_The_Stored_View_Model()
        {
            var vmType = typeof(TestViewModel);
            Route.SetViewModel(vmType);
            Route.ViewModelType.ShouldBeEquivalentTo(vmType);
        }

        [Fact]
        public void Test_SetViewModel_Returns_A_Route_Builder()
        {
            var vmType = typeof(TestViewModel);
            var ret = Route.SetViewModel(vmType);
            ret.Should().Be(Route);
        }

        [Fact]
        public void Test_Present_Adds_Presenter_Type_To_Presenters()
        {
            Route.SetViewModel(typeof(TestViewModel))
                .Present(typeof(TestPresenterType));
            Assert.Collection(Route.Presenters,
                p => p.Should().Be(typeof(TestPresenterType)));
        }

        [Fact]
        public void Test_Navigate_Adds_Func_To_NavigationActions()
        {
            Route.Navigate();
            Assert.Collection(Route.NavigationActions,
                action => action.Should().NotBeNull());
        }

        [Fact]
        public void Test_Navigate_Adds_Func_That_Calls_Push_Async_On_Given_Navigator()
        {
            var navigator = Substitute.For<INavigator>();
            Route.Navigate();
            Route.SetViewModel(typeof(TestViewModel));
            Route.NavigationActions.First()(navigator, new Transition());
            navigator.Received(1).PushAsync(Arg.Any<Transition>());
        }

        [Fact]
        public void Test_NavigateBackWhile_Adds_Func_To_NavigationActions()
        {
            Route.NavigateBackWhile(vm => true);
            Assert.Collection(Route.NavigationActions,
                action => action.Should().NotBeNull());
        }

        [Fact]
        public async Task Test_NavigateBackWhile_Adds_Func_That_Pops_All_Transitions_That_Do_Not_Match_Predicate()
        {
            var viewModels = new[]
            {
                new TestViewModel(),
                new TestViewModel(),
                new object(),
            };
            var transitions = viewModels.Select(vm => new Transition()
            {
                ViewModel = vm
            }).ToArray();
            var navigator = Substitute.For<INavigator>();
            navigator.Peek().Returns(transitions.First(), transitions.Skip(1).ToArray());
            navigator.PopAsync().Returns(transitions.First(), transitions.Skip(1).ToArray());

            Route.NavigateBackWhile(vm => vm is TestViewModel);

            await Route.NavigationActions.First()(navigator, new Transition());

            navigator.Received(2).PopAsync();
        }

        [Fact]
        public async Task Test_NavigateBackWhile_Adds_Func_That_Pops_Until_No_More_Transitions_Are_Available()
        {
            var viewModels = new[]
            {
                new TestViewModel(),
                new TestViewModel()
            };
            var transitions = viewModels.Select(vm => new Transition()
            {
                ViewModel = vm
            }).Concat(new Transition[] { null }).ToArray();
            var navigator = Substitute.For<INavigator>();
            navigator.Peek().Returns(transitions.First(), transitions.Skip(1).ToArray());
            navigator.PopAsync().Returns(transitions.First(), transitions.Skip(1).ToArray());

            Route.NavigateBackWhile(vm => vm is TestViewModel);

            await Route.NavigationActions.First()(navigator, new Transition());

            navigator.Received(2).PopAsync();
        }

        [Fact]
        public async Task Test_Build_Creates_RouteActions_With_NavigationAction()
        {
            Route.SetViewModel(typeof(TestViewModel))
                .Navigate();
            var navigator = Substitute.For<INavigator>();
            var actions = Route.Build();
            var transition = new Transition()
            {
                ViewModel = new TestViewModel()
            };
            await actions.NavigationAction(navigator, transition);


            navigator.Received(1).PushAsync(transition);
        }

        [Fact]
        public async Task Test_Build_Creates_RouteActions_With_NavigationAction_For_Multiple_Actions()
        {
            var viewModels = new[]
            {
                new TestViewModel(),
                new TestViewModel(),
                new object(),
            };
            var transitions = viewModels.Select(vm => new Transition()
            {
                ViewModel = vm
            }).ToArray();
            var navigator = Substitute.For<INavigator>();
            navigator.Peek().Returns(transitions.First(), transitions.Skip(1).ToArray());
            navigator.PopAsync().Returns(transitions.First(), transitions.Skip(1).ToArray());
            var transition = new Transition()
            {
                ViewModel = new TestViewModel()
            };
            Route
                .SetViewModel(typeof(TestViewModel))
                .NavigateBackWhile(vm => vm is TestViewModel)
                .Navigate();

            var actions = Route.Build();
            await actions.NavigationAction(navigator, transition);

            navigator.Received(2).PopAsync();
            navigator.Received(1).PushAsync(transition);
        }

        [Fact]
        public async Task Test_Build_Creates_RouteActions_With_Presenters()
        {
            Route.SetViewModel(typeof(TestViewModel))
                .Present<TestPresenterType>()
                .Present<OtherTestPresenterType>();

            var actions = Route.Build();

            Assert.Collection(actions.Presenters,
                p => p.Should().Be(typeof(TestPresenterType)),
                p => p.Should().Be(typeof(OtherTestPresenterType)));
        }

        [Fact]
        public async Task Test_Build_Creates_RouteActions_With_Empty_Presenters()
        {
            Route.SetViewModel(typeof(TestViewModel));

            var actions = Route.Build();
            actions.Presenters.Should().BeEmpty();
        }
    }
}
#pragma warning restore 4014
