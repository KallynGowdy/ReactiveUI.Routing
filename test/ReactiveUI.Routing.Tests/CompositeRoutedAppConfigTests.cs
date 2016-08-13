using System;
using NSubstitute;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class CompositeRoutedAppConfigTests : LocatorTest
    {
        [Fact]
        public void Test_Calls_Register_Dependencies_On_Each_Sub_Config()
        {
            var config1 = Substitute.For<IRoutedAppConfig>();
            var config2 = Substitute.For<IRoutedAppConfig>();
            var config3 = Substitute.For<IRoutedAppConfig>();

            var config = new CompositeRoutedAppConfig(config1, config2, config3);

            config.RegisterDependencies(Resolver);

            config1.Received(1).RegisterDependencies(Resolver);
            config2.Received(1).RegisterDependencies(Resolver);
            config3.Received(1).RegisterDependencies(Resolver);
        }

        [Fact]
        public void Test_Ignores_Null_Configs()
        {
            var c = Substitute.For<IRoutedAppConfig>();
            var config = new CompositeRoutedAppConfig(null, c, null);
            config.RegisterDependencies(Resolver);
        }

        [Fact]
        public void Test_Calls_CloseApp_On_The_Last_Config()
        {
            var config1 = Substitute.For<IRoutedAppConfig>();
            var config2 = Substitute.For<IRoutedAppConfig>();
            var config3 = Substitute.For<IRoutedAppConfig>();

            var config = new CompositeRoutedAppConfig(config1, config2, config3);

            config.CloseApp();

            config1.DidNotReceive().CloseApp();
            config2.DidNotReceive().CloseApp();
            config3.Received(1).CloseApp();
        }

        [Fact]
        public void Test_Throws_ArgumentException_From_Constructor_If_No_Configs_Implement_CloseApp()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var config = new CompositeRoutedAppConfig();
            });
        }

    }
}