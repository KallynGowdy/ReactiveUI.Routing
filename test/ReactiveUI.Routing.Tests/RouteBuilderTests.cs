using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Actions;
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
            Assert.Collection(Route.Build().Actions,
                p => p.As<PresentRouteAction>().PresenterType.Should().Be<TestPresenterType>());
        }

        [Fact]
        public void Test_NavigateBackWhile_Adds_NavigateBackWhileRouteAction()
        {
            Route.NavigateBackWhile(vm => vm.ViewModel is TestViewModel);
            var actions = Route.Build();

            Assert.Collection(actions.Actions,
                a => a.As<NavigateBackWhileRouteAction>()
                .GoBackWhile.Should().NotBeNull());
        }

        [Fact]
        public void Test_Build_Creates_RouteActions_With_NavigationRouteAction()
        {
            Route.SetViewModel(typeof(TestViewModel))
                .Navigate();
            var actions = Route.Build();

            Assert.Collection(actions.Actions,
                a => a.Should().BeOfType<NavigateRouteAction>());
        }

        [Fact]
        public void Test_Build_Creates_RouteActions_With_NavigationAction_For_Multiple_Actions()
        {
            Route
                .SetViewModel(typeof(TestViewModel))
                .NavigateBackWhile(vm => vm.ViewModel is TestViewModel)
                .Navigate();

            var actions = Route.Build();
            Assert.Collection(actions.Actions,
                a => a.Should().BeOfType<NavigateBackWhileRouteAction>(),
                a => a.Should().BeOfType<NavigateRouteAction>());
        }

        [Fact]
        public void Test_Build_Creates_RouteActions_With_Presenters()
        {
            Route.SetViewModel(typeof(TestViewModel))
                .Present<TestPresenterType>()
                .Present<OtherTestPresenterType>();

            var actions = Route.Build();

            Assert.Collection(actions.Actions,
                p => p.As<PresentRouteAction>().PresenterType.Should().Be<TestPresenterType>(),
                p => p.As<PresentRouteAction>().PresenterType.Should().Be<OtherTestPresenterType>());
        }

        [Fact]
        public void Test_Build_Creates_RouteActions_With_Empty_Actions()
        {
            Route.SetViewModel(typeof(TestViewModel));

            var actions = Route.Build();
            actions.Actions.Should().BeEmpty();
        }

        [Fact]
        public void Test_Build_Creates_RouteActions_With_ViewModel()
        {
            Route.SetViewModel(typeof(TestViewModel));
            var actions = Route.Build();
            actions.ViewModelType.Should().Be(typeof(TestViewModel));
        }
    }
}
#pragma warning restore 4014
