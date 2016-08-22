using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ReactiveUI.Routing.Actions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class RouterActionTests
    {
        [Theory]
        [MemberData(nameof(ShowViewModelActionParams))]
        [MemberData(nameof(NavigateBackActionParams))]
        [MemberData(nameof(ShowDefaultViewModelActionParams))]
        public void Test_Actions_Are_Equal_When_Their_Parameters_Are_Equal(IRouterAction first, IRouterAction second, bool areEqual)
        {
            if (areEqual)
            {
                first.Should().Be(second);
                second.Should().Be(first);
            }
            else
            {
                first.Should().NotBe(second);
                second.Should().NotBe(first);
            }
        }

        public static object[][] ShowDefaultViewModelActionParams => new object[][]
        {
            new object[]
            {
                new ShowDefaultViewModelAction(), 
                new ShowDefaultViewModelAction(),
                true
            },
            new object[]
            {
                new ShowDefaultViewModelAction(),
                null,
                false
            },
            new object[]
            {
                null,
                new ShowDefaultViewModelAction(),
                false
            }
        };

        public static object[][] NavigateBackActionParams => new object[][]
        {
            new object[]
            {
                new NavigateBackAction(), 
                new NavigateBackAction(), 
                true
            },
            new object[]
            {
                new NavigateBackAction(),
                null,
                false
            },
            new object[]
            {
                null,
                new NavigateBackAction(),
                false
            }
        };

        public static object[][] ShowViewModelActionParams => new object[][]
        {
            ShowViewModelAction(null, null, true),
            ShowViewModelAction(new ActivationParams(), null, false),
            ShowViewModelAction(null, new ActivationParams(), false),
            ShowViewModelAction(new ActivationParams(), new ActivationParams(), true),
        };

        private static object[] ShowViewModelAction(ActivationParams first, ActivationParams second, bool areEqual) =>
            new object[]
            {
                new ShowViewModelAction()
                {
                    ActivationParams = first
                },
                new ShowViewModelAction()
                {
                    ActivationParams = second
                },
                areEqual
            };
    }
}
