using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Splat;
using Xunit;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace ReactiveUI.Routing.Tests
{
    public class TestRoutedAppConfig : DefaultRoutedAppConfig
    {
        public RouterParams RouterParams { get; set; } = new RouterParams();

        protected override RouterParams BuildRouterParams()
        {
            return RouterParams;
        }
    }

    public class RoutedAppHostTests : LocatorTest
    {
        public IRoutedAppHost AppHost { get; set; }
        private IRoutedAppConfig Config { get; set; }

        public RoutedAppHostTests()
        {
            Config = Substitute.For<IRoutedAppConfig>();
            AppHost = new RoutedAppHost(Config);
        }

        private void Register<T>(Func<T> valueFactory)
        {
            Config.When(c => c.RegisterDependencies(Arg.Any<IMutableDependencyResolver>()))
                .Do(ci => ci.Arg<IMutableDependencyResolver>().Register(() => valueFactory(), typeof(T)));
        }

        private void Register<T>(T value)
        {
            Config.When(c => c.RegisterDependencies(Arg.Any<IMutableDependencyResolver>()))
                .Do(ci => ci.Arg<IMutableDependencyResolver>().Register(() => value, typeof(T)));
        }

        [Fact]
        public void Test_Start_Calls_Register_Dependencies_On_Config()
        {
            Register(new RouterParams());
            Register(Substitute.For<IActivator>());
            Register(Substitute.For<IRouter>());
            AppHost.Start();
            Config.Received(1).RegisterDependencies(Arg.Any<IMutableDependencyResolver>());
        }

        [Fact]
        public void Test_Start_Calls_Resolves_RouterParams_On_Start()
        {
            var buildRouterParams = Substitute.For<Func<RouterParams>>();
            buildRouterParams().Returns(new RouterParams());
            Register(buildRouterParams);
            Register(Substitute.For<IActivator>());
            Register(Substitute.For<IRouter>());
            AppHost.Start();
            buildRouterParams.Received(1)();
        }

        [Fact]
        public async Task Test_StartAsync_Throws_InvalidOperationException_If_Resolved_RouterParams_Is_Null()
        {
            Register(Substitute.For<IActivator>());
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await AppHost.StartAsync();
            });
        }

        [Fact]
        public async Task Test_StartAsync_Throws_InvalidOperationException_If_Resolved_IActivator_Is_Null()
        {
            Register(new RouterParams());
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await AppHost.StartAsync();
            });
        }

        [Fact]
        public async Task Test_StartAsync_Calls_Activate_Async_On_IActivator_For_Router()
        {
            var activator = Substitute.For<IActivator>();
            Register(new RouterParams());
            Register(activator);
            await AppHost.StartAsync();


            activator.Received(1)
                .ActivateAsync(Arg.Is<ActivationParams>(p => p.Type == typeof(IRouter) && p.Params is RouterParams));
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
