using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Builder;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class TestViewModel
    {
    }

    public class TestPresenterType
    {
    }

    public class RouteBuilderTests
    {
        IRouteBuilder route;

        public RouteBuilderTests()
        {
            route = new RouteBuilder();
            route.NavigationActions.Should().BeEmpty();
            route.Presenters.Should().BeEmpty();
            route.ViewModelType.Should().BeNull();
        }

        [Fact]
        public void Test_SetViewModel_Sets_The_Stored_View_Model()
        {
            var vmType = typeof(TestViewModel);
            route.SetViewModel(vmType);
            route.ViewModelType.ShouldBeEquivalentTo(vmType);
        }

        [Fact]
        public void Test_SetViewModel_Returns_A_Route_Builder()
        {
            var vmType = typeof(TestViewModel);
            var ret = route.SetViewModel(vmType);
            ret.Should().Be(route);
        }

        [Fact]
        public void Test_Present_Adds_Presenter_Type_To_Presenters()
        {
            route.SetViewModel(typeof(TestViewModel))
                .Present(typeof(TestPresenterType));
            Assert.Collection(route.Presenters,
                p => p.Should().Be(typeof(TestPresenterType)));
        }

        [Fact]
        public void Test_Navigate_Adds_Func_To_NavigationActions()
        {
            route.Navigate();
            Assert.Collection(route.NavigationActions,
                action => action.Should().NotBeNull());
        }

        [Fact]
        public void Test_Navigate_Adds_Func_That_Calls_Push_Async_On_Given_Navigator()
        {
            var navigator = Substitute.For<INavigator>();
            var parameters = new object();
            route.Navigate();
            route.SetViewModel(typeof(TestViewModel));
            route.NavigationActions.First()(navigator, new ActivationParams()
            {
                Params = parameters,
                Type = null
            });

            navigator.Received(1).PushAsync(Arg.Is<ActivationParams>(p => p.Params == parameters && p.Type == typeof(TestViewModel)));
        }

        [Fact]
        public void Test_NavigateBackWhile_Adds_Func_To_NavigationActions()
        {
            route.NavigateBackWhile(vm => true);
            Assert.Collection(route.NavigationActions,
                action => action.Should().NotBeNull());
        }

        [Fact]
        public void Test_NavigateBackWhile_Adds_Func_That_Pops_All_Transitions_That_Do_Not_Match_Predicate()
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

            route.NavigateBackWhile(vm => vm is TestViewModel);

            route.NavigationActions.First()(navigator, new ActivationParams()
            {
                Params = new object()
            });

            navigator.Received(2).PopAsync();
        }
    }
}
