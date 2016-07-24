using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Splat;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class RoutedAppHostTests : LocatorTest
    {
        public IRoutedAppHost AppHost { get; set; }
        private IRoutedAppConfig Config { get; }

        public RoutedAppHostTests()
        {
            Config = Substitute.For<IRoutedAppConfig>();
            AppHost = new RoutedAppHost(Config);
        }

        [Fact]
        public void Test_Start_Calls_Register_Dependencies_On_Config()
        {
            AppHost.Start();
            Config.Received(1).RegisterDependencies(Arg.Any<IMutableDependencyResolver>());
        }

        [Fact]
        public void Test_Start_Calls_BuildRouterAsync_On_Config()
        {
            AppHost.Start();
            Config.Received(1).BuildRouterAsync();
        }
    }
}
