using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing.Configuration;
using ReactiveUI.Routing.Presentation;
using Splat;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests
{
    public class ReactiveAppBuilderTests
    {
        public IReactiveAppBuilder Subject { get; set; }

        public ReactiveAppBuilderTests()
        {
            Subject = new ReactiveAppBuilder();
        }

        [Fact]
        public void Test_Add_Checks_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Subject.Add(null);
            });
        }

        [Fact]
        public void Test_Apply_Checks_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Subject.Apply(null);
            });
        }

        [Fact]
        public void Test_Apply_Calls_Apply_On_Objects_Registered_With_Add()
        {
            var dependency1 = Substitute.For<IReactiveAppDependency>();
            var dependency2 = Substitute.For<IReactiveAppDependency>();
            var dependency3 = Substitute.For<IReactiveAppDependency>();
            var resolver = Substitute.For<IMutableDependencyResolver>();

            Subject.Add(dependency1);
            Subject.Add(dependency3);
            Subject.Add(dependency2);

            Subject.Apply(resolver);

            dependency1.Received(1).Apply(resolver);
            dependency2.Received(1).Apply(resolver);
            dependency3.Received(1).Apply(resolver);
        }

        [Fact]
        public void Test_BuildTypesToLoadList_Returns_Expected_Type_Names()
        {
            var list = ReactiveAppBuilderExtensions.BuildTypesToLoadList();

            Assert.Collection(list,
                type => Assert.Equal("ReactiveUI.Routing.CoreRoutingDependencies, ReactiveUI.Routing.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", type),
                type => Assert.Equal("ReactiveUI.Routing.Android.AndroidRoutingDependencies, ReactiveUI.Routing.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", type),
                type => Assert.Equal("ReactiveUI.Routing.UWP.UwpRoutingDependencies, ReactiveUI.Routing.UWP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", type));
        }

        [Fact]
        public void Test_Build_Returns_A_New_ReactiveApp()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveUI()
                .AddReactiveRouting()
                .Build();

            Assert.NotNull(app);
            Assert.NotNull(app.Presenter);
            Assert.NotNull(app.Router);
            Assert.NotNull(app.SuspensionHost);
            Assert.NotNull(app.SuspensionDriver);
            Assert.NotNull(app.Locator);
        }

        [Fact]
        public void Test_Build_Without_ReactiveUI_Dependencies_Defaults_To_RxApp_SuspensionHost()
        {
            var before = RxApp.SuspensionHost;
            try
            {
                var host = RxApp.SuspensionHost = new DefaultSuspensionHost();

                var app = new ReactiveAppBuilder()
                    .AddReactiveRouting()
                    .Build();

                Assert.Same(host, app.SuspensionHost);
            }
            finally
            {
                RxApp.SuspensionHost = before;
            }
        }

        [Fact]
        public void Test_Configurations_Are_Called_On_App_After_It_Is_Built()
        {
            IReactiveApp hitApp = null;
            var config = new ActionConfiguration(a => hitApp = a);

            var app = new ReactiveAppBuilder()
                .AddReactiveUI()
                .AddReactiveRouting()
                .Configure(config)
                .Build();

            Assert.Same(app, hitApp);
        }

        [Fact]
        public void Test_Build_Adds_ReactiveApp_To_Locator()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveUI()
                .AddReactiveRouting()
                .Build();

            var located = Locator.CurrentMutable.GetService<IReactiveApp>();
            var located2 = Locator.CurrentMutable.GetService<ReactiveApp>();

            Assert.Same(app, located);
            Assert.Same(app, located2);
        }

        [Fact]
        public void Test_Build_Adds_Presenter_To_Locator()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveUI()
                .AddReactiveRouting()
                .Build();

            var located = Locator.CurrentMutable.GetService<IAppPresenter>();

            Assert.Same(app.Presenter, located);
        }

        [Fact]
        public void Test_Build_Adds_Router_To_Locator()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveUI()
                .AddReactiveRouting()
                .Build();

            var located = Locator.CurrentMutable.GetService<IReactiveRouter>();

            Assert.Same(app.Router, located);
        }
    }
}
