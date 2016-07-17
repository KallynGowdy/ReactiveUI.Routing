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
    public class RouterBuilderTests
    {
        readonly IRouterBuilder builder;

        public RouterBuilderTests()
        {
            this.builder = new RouterBuilder(() => Substitute.For<INavigator>(), () => Substitute.For<IPresenter>());
        }

        [Fact]
        public void Test_When_Calls_Provided_Callback()
        {
            Func<IRouteBuilder, IRouteBuilder> routeBuilder =
                Substitute.For<Func<IRouteBuilder, IRouteBuilder>>();
            builder.When(routeBuilder);
            routeBuilder.Received(1)(Arg.Any<IRouteBuilder>());
        }

        [Fact]
        public void Test_When_Sends_IRouteBuilder_To_Provided_Callback()
        {
            builder.When(route =>
            {
                route.Should().NotBeNull();
                return route;
            });
        }

        [Fact]
        public void Test_Generic_When_With_Callback_Sets_View_Model_On_RouteBuilder()
        {
            builder.When<TestViewModel>(route =>
            {
                route.ViewModelType.Should().Be<TestViewModel>();
                return route;
            });
        }

        [Fact]
        public void Test_Generic_When_Sets_View_Model_On_RouteBuilder()
        {
            builder.When<TestViewModel>();
            Assert.Collection(builder.BuiltRoutes,
                route => route.ViewModelType.Should().Be<TestViewModel>());
        }

        [Fact]
        public void Test_Build_Returns_A_Router()
        {
            var router = builder.Build();
            router.Should().NotBeNull();
        }
    }
}
