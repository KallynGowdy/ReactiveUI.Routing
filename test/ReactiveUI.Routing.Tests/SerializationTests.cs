using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using ReactiveUI.Routing.Actions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class SerializationTests
    {

        [Fact]
        public void Test_Can_Serialize_RouterParams()
        {
            var str = JsonConvert.SerializeObject(new RouterConfig()
            {
                DefaultParameters = new TestParams(),
                DefaultViewModelType = typeof(TestViewModel),
                ViewModelMap = new Dictionary<Type, RouteActions>()
                {
                    {
                        typeof(TestViewModel),
                        new RouteActions()
                        {
                            Actions = new IRouteAction[]
                            {
                                RouteActions.Navigate()
                            }
                        }
                    }
                }
            }, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            str.Should().NotBeNull();
        }

    }
}
