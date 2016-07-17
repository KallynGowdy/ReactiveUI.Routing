using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class ActivatableObjectTests
    {
        protected IActivatable<TestParams> Obj { get; set; }

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
    }
}
