using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class ActivatableObjectTests
    {
        class TestActivatableObject : ActivatableObject<TestParams>
        {
            public new bool Initialized => base.Initialized;
        }

        protected ActivatableObject<TestParams> Obj { get; set; }

        public ActivatableObjectTests()
        {
            Obj = new ActivatableObject<TestParams>();
        }

        [Fact]
        public async Task Test_InitAsync_Throws_When_Given_Null_Params()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Obj.InitAsync(null);
            });
        }

        [Fact]
        public async Task Test_InitAsync_Does_Not_Throw_When_Given_Non_Null_Params()
        {
            await Obj.InitAsync(new TestParams());
        }

        [Fact]
        public async Task Test_DestroyAsync_Does_Not_Throw()
        {
            await Obj.DestroyAsync();
        }

        [Fact]
        public async Task Test_Initialized_Is_Set_To_True_After_InitAsync_Is_Called()
        {
            var obj = new TestActivatableObject();
            await obj.InitAsync(new TestParams());
            obj.Initialized.Should().BeTrue();
        }
    }
}
