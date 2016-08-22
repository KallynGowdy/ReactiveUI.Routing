using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class AppConfigTests : LocatorTest
    {
        [Fact]
        public void Test_RegisterDependencies_Throws_InvalidOperationException_If_Configs_Dont_Register_Required_Dependencies()
        {
            var c = Substitute.For<IRoutedAppConfig>();
            var config = new AppConfig(c);
            Assert.Throws<InvalidOperationException>(() =>
            {
                config.RegisterDependencies(Resolver);
            });
        }
    }
}
