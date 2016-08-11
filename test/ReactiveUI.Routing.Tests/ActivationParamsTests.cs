using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using FluentAssertions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class ActivationParamsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(typeof(TestViewModel), "ReactiveUI.Routing.Tests.TestViewModel, ReactiveUI.Routing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
        public void Test_TypeName_Stores_The_Name_Of_The_Stored_Type(Type type, string expected)
        {
            var p = new ActivationParams()
            {
                Type = type
            };
            p.GetPropertyValue("TypeName").Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("ReactiveUI.Routing.Tests.TestViewModel, ReactiveUI.Routing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(TestViewModel))]
        public void Test_Setting_TypeName_Stores_Correct_Type(string storedType, Type expected)
        {
            var p = new ActivationParams();
            p.SetPropertyValue("TypeName", storedType);
            p.Type.ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(typeof(TestViewModel), typeof(TestViewModel), true)]
        [InlineData(typeof(TestViewModel), null, false)]
        [InlineData(null, typeof(TestViewModel), false)]
        [InlineData(typeof(OtherTestPresenterType), typeof(TestViewModel), false)]
        public void Test_ActivationParams_Are_Equal_When_Their_Types_Are_Equal(Type first, Type second, bool areEqual)
        {
            var f = new ActivationParams()
            {
                Type = first
            };
            var s = new ActivationParams()
            {
                Type = second
            };

            if (areEqual)
            {
                f.Should().Be(s);
                s.Should().Be(f);
            }
            else
            {
                f.Should().NotBe(s);
                s.Should().NotBe(f);
            }
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData("Equal", "Equal", true)]
        [InlineData("NotEqual", null, false)]
        [InlineData(null, "NotEqual", false)]
        [InlineData("NotEqual", "Other", false)]
        public void Test_ActivationParams_Are_Equal_When_Their_Parameters_Are_Equal(object first, object second,
            bool areEqual)
        {
            var f = new ActivationParams()
            {
                Params = first
            };
            var s = new ActivationParams()
            {
                Params = second
            };

            if (areEqual)
            {
                f.Should().Be(s);
                s.Should().Be(f);
            }
            else
            {
                f.Should().NotBe(s);
                s.Should().NotBe(f);
            }
        }

    }
}
