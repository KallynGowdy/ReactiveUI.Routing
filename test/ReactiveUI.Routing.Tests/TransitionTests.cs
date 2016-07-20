using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Splat;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class TransitionTests : LocatorTest
    {
        private readonly Transition transition;

        public TransitionTests()
        {
            transition = new Transition();
        }

        [Fact]
        public async Task Test_Uses_Locator_To_Instantiate_View_Models()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            await transition.InitAsync(new ActivationParams()
            {
                Type = typeof(TestViewModel),
                Params = new TestParams()
            });

            transition.ViewModel.Should().BeAssignableTo<TestViewModel>();
        }

        [Fact]
        public async Task Test_Throws_When_View_Model_Is_Not_Registered()
        {
            await Assert.ThrowsAsync<InvalidReturnValueException>(async () =>
            {
                await transition.InitAsync(new ActivationParams()
                {
                    Type = typeof(TestViewModel),
                    Params = new TestParams()
                });
            });
        }

        [Fact]
        public async Task Test_Initializes_New_View_Model()
        {
            Locator.CurrentMutable.Register(() => new TestViewModel(), typeof(TestViewModel));
            await transition.InitAsync(new ActivationParams()
            {
                Type = typeof(TestViewModel),
                Params = new TestParams()
            });

            ((TestViewModel) transition.ViewModel).Initialized.Should().BeTrue();
        }
    }
}
