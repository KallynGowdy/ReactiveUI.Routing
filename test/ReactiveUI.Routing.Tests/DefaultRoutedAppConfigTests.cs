using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class DefaultRoutedAppConfigTests : LocatorTest
    {
        public class TestNullRoutedAppConfig : DefaultRoutedAppConfig
        {
            public override void CloseApp()
            {
                throw new NotImplementedException();
            }

            protected override RouterConfig BuildRouterParams()
            {
                return null;
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

        [Fact]
        public void Test_RegisterDependencies_Throws_InvalidReturnValueException_If_BuildRouterParams_Returns_Null()
        {
            var config = new TestNullRoutedAppConfig();
            config.RegisterDependencies(Resolver);
        }

        [Fact]
        public void Test_RegisterDependencies_ArgumentNullException_For_Given_Resolver()
        {
            var config = new TestNullRoutedAppConfig();
            Assert.Throws<ArgumentNullException>("resolver", () => config.RegisterDependencies(null));
        }
    }
}
