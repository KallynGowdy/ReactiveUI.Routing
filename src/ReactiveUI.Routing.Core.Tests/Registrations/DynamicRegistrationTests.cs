using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Registrations;
using Splat;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests.Registrations
{
    public class DynamicRegistrationTests
    {
        private class TestRegistrations : IReactiveAppDependency
        {
            public static TestRegistrations Created { get; set; }

            public IMutableDependencyResolver CalledResolver { get; set; }

            public TestRegistrations()
            {
                Created = this;
            }

            public void Apply(IMutableDependencyResolver resolver)
            {
                CalledResolver = resolver;
            }
        }

        public DynamicRegistration Subject { get; set; }

        [Fact]
        public void Test_Apply_Looks_Up_TypeName_Creates_Type_And_Applies_Registrations()
        {
            var resolver = Substitute.For<IMutableDependencyResolver>();
            
            Subject = new DynamicRegistration(typeof(TestRegistrations).AssemblyQualifiedName);

            Subject.Apply(resolver);

            TestRegistrations.Created.Should().NotBeNull();
            TestRegistrations.Created.CalledResolver.Should().BeSameAs(resolver);
        }

        [Fact]
        public void Test_Apply_Does_Nothing_If_Type_Is_Not_Found()
        {
            var resolver = Substitute.For<IMutableDependencyResolver>();

            Subject = new DynamicRegistration("NonExistantType");

            Subject.Apply(resolver);

            TestRegistrations.Created.Should().BeNull();
        }
    }
}
