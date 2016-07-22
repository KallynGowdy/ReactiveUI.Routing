using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class ReActivatableObjectTests : ActivatableObjectTests
    {
        public class TestSyncReActivatablObject : ReActivatableObject<TestParams, TestState>
        {
            public new IObservable<TestState> Resumed => base.Resumed;

            protected override TestState SuspendCoreSync()
            {
                return null;
            }
        }

        public class TestAsyncReActivatablObject : ReActivatableObject<TestParams, TestState>
        {
            protected override Task<TestState> SuspendCoreAsync()
            {
                return Task.FromResult<TestState>(null);
            }
        }

        protected ReActivatableObject<TestParams, TestState> ReObj => (ReActivatableObject<TestParams, TestState>)Obj;

        public ReActivatableObjectTests()
        {
            Obj = new ReActivatableObject<TestParams, TestState>();
        }

        [Fact]
        public async Task Test_ResumeAsync_Throws_If_Given_Null_State()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await ReObj.ResumeAsync(null);
            });
        }

        [Fact]
        public async Task Test_ResumeAsync_Does_Not_Throw_If_Given_Non_Null_State()
        {
            await ReObj.ResumeAsync(new TestState());
        }

        [Fact]
        public async Task Test_SuspendAsync_Throws_If_SuspendCoreSync_Returns_Null_State()
        {
            Obj = new TestSyncReActivatablObject();
            await Assert.ThrowsAsync<InvalidReturnValueException>(async () =>
            {
                await ReObj.SuspendAsync();
            });
        }

        [Fact]
        public async Task Test_SuspendAsync_Throws_If_SuspendCoreAsync_Returns_Null_State()
        {
            Obj = new TestAsyncReActivatablObject();
            await Assert.ThrowsAsync<InvalidReturnValueException>(async () =>
            {
                await ReObj.SuspendAsync();
            });
        }

        [Fact]
        public async Task Test_SuspendAsync_Does_Not_Throw_If_Returns_Non_Null_State()
        {
            await ReObj.SuspendAsync();
        }

        [Fact]
        public async Task Test_Resumed_Emits_State_After_ResumeCoreAsync_Is_Called()
        {
            var obj = new TestSyncReActivatablObject();
            var first = new TestState();
            var second = new TestState();
            List<TestState> resumedStates = new List<TestState>(2);
            obj.Resumed.Subscribe(state => resumedStates.Add(state));

            await obj.ResumeAsync(first);
            await obj.ResumeAsync(second);

            Assert.Collection(resumedStates,
                s => s.Should().Be(first),
                s => s.Should().Be(second));
        }

        [Fact]
        public async Task Test_Resumed_Emits_Most_Recent_State_For_Additional_Subscriptions()
        {
            var obj = new TestSyncReActivatablObject();
            var first = new TestState();
            List<TestState> resumedStates = new List<TestState>(2);
            Action subscribe = () => obj.Resumed.Subscribe(state => resumedStates.Add(state));
            await obj.ResumeAsync(first);

            subscribe();
            subscribe();
            subscribe();

            Assert.Collection(resumedStates,
                s => s.Should().Be(first),
                s => s.Should().Be(first),
                s => s.Should().Be(first));
        }
    }
}
