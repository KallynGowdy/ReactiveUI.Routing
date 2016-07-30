using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

        protected override ISuspensionNotifier BuildSuspensionNotifier()
        {
            throw new NotImplementedException();
        }

        protected override IObjectStateStore BuildObjectStateStore()
        {
            throw new NotImplementedException();
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
            Register<IReActivator>(ReActivator.Current);
            Register(Substitute.For<IRouter>());
            Register(Substitute.For<ISuspensionNotifier>());
            Register(Substitute.For<IObjectStateStore>());
            AppHost.Start();
            Config.Received(1).RegisterDependencies(Arg.Any<IMutableDependencyResolver>());
        }

        [Fact]
        public void Test_Start_Calls_Resolves_RouterParams_On_Start()
        {
            var buildRouterParams = Substitute.For<Func<RouterParams>>();
            buildRouterParams().Returns(new RouterParams());
            Register(buildRouterParams);
            Register<IReActivator>(ReActivator.Current);
            Register(Substitute.For<IRouter>());
            Register(Substitute.For<ISuspensionNotifier>());
            Register(Substitute.For<IObjectStateStore>());
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
        public async Task Test_StartAsync_Calls_InitAsync_On_IRouter()
        {
            var activator = Substitute.For<IReActivator>();
            var router = Substitute.For<IRouter>();
            var routerParams = new RouterParams();
            Register(routerParams);
            Register(activator);
            Register(Substitute.For<ISuspensionNotifier>());
            Register(Substitute.For<IObjectStateStore>());
            Register(router);
            await AppHost.StartAsync();

            router.Received(1).InitAsync(routerParams);
        }

        [Fact]
        public async Task Test_StartAsync_Calls_ResumeAsync_On_IRouter_When_State_Is_Available()
        {
            var activator = Substitute.For<IReActivator>();
            var router = Substitute.For<IRouter>();
            var stateStore = Substitute.For<IObjectStateStore>();
            var routerParams = new RouterParams();
            var routerState = new RouterState();
            var state = new ObjectState()
            {
                State = routerState
            };

            stateStore.LoadStateAsync().Returns(state);
            Register(routerParams);
            Register(activator);
            Register(Substitute.For<ISuspensionNotifier>());
            Register(stateStore);
            Register(router);
            await AppHost.StartAsync();

            router.Received(1).ResumeAsync(routerState, activator);
        }

        [Fact]
        public async Task Test_Clears_Stored_State_When_ResumeAsync_Fails_On_Router()
        {
            var activator = Substitute.For<IReActivator>();
            var router = Substitute.For<IRouter>();
            var stateStore = Substitute.For<IObjectStateStore>();
            var suspensionNotifier = Substitute.For<ISuspensionNotifier>();
            var routerParams = new RouterParams();
            var routerState = new RouterState();
            var state = new ObjectState()
            {
                State = routerState
            };

            suspensionNotifier.OnSaveState.Returns(Observable.Never<Unit>());
            suspensionNotifier.OnSuspend.Returns(Observable.Never<Unit>());
            stateStore.LoadStateAsync().Returns(state);
            router.ResumeAsync(Arg.Any<RouterState>(), activator).Throws<Exception>();
            Register(routerParams);
            Register(activator);
            Register(suspensionNotifier);
            Register(stateStore);
            Register(router);
            await AppHost.StartAsync();

            stateStore.Received(1).SaveStateAsync(null);
        }

        [Fact]
        public async Task Test_Clears_Stored_State_When_Not_Castable_To_RouterState()
        {
            var activator = Substitute.For<IReActivator>();
            var router = Substitute.For<IRouter>();
            var stateStore = Substitute.For<IObjectStateStore>();
            var suspensionNotifier = Substitute.For<ISuspensionNotifier>();
            var routerParams = new RouterParams();
            var notCastable = new object();
            var state = new ObjectState()
            {
                State = notCastable
            };

            suspensionNotifier.OnSaveState.Returns(Observable.Never<Unit>());
            suspensionNotifier.OnSuspend.Returns(Observable.Never<Unit>());
            stateStore.LoadStateAsync().Returns(state);
            Register(routerParams);
            Register(activator);
            Register(suspensionNotifier);
            Register(stateStore);
            Register(router);
            await AppHost.StartAsync();

            stateStore.Received(1).SaveStateAsync(null);
        }

        [Fact]
        public async Task Test_Clears_Stored_State_When_ObjectStateStore_Throws()
        {
            var activator = Substitute.For<IReActivator>();
            var router = Substitute.For<IRouter>();
            var stateStore = Substitute.For<IObjectStateStore>();
            var suspensionNotifier = Substitute.For<ISuspensionNotifier>();
            var routerParams = new RouterParams();

            suspensionNotifier.OnSaveState.Returns(Observable.Never<Unit>());
            suspensionNotifier.OnSuspend.Returns(Observable.Never<Unit>());
            stateStore.LoadStateAsync().Throws<Exception>();
            Register(routerParams);
            Register(activator);
            Register(suspensionNotifier);
            Register(stateStore);
            Register(router);
            await AppHost.StartAsync();

            stateStore.Received(1).SaveStateAsync(null);
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
