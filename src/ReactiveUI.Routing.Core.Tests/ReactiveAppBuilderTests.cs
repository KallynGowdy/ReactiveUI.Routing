using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
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
                type => Assert.Equal("ReactiveUI.Routing.Android.AndroidRoutingDependencies, ReactiveUI.Routing.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", type));
        }
    }
}
