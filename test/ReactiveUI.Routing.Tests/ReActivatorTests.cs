using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace ReactiveUI.Routing.Tests
{
    public class ReActivatorTests : LocatorTest
    {
        private ReActivator ReActivator { get; set; }
        private IActivator Activator { get; set; }

        public ReActivatorTests()
        {
            Activator = Substitute.For<IActivator>();
            ReActivator = new ReActivator(Activator);
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_ActivationParams_For_Regular_Object()
        {
            var obj = new object();
            var state = await ReActivator.SuspendAsync(obj);
            state.Params.Type.Should().Be<object>();
            state.Params.Params.Should().BeNull();
            state.State.Should().BeNull();
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_ActivationParams_For_IActivatable_Object()
        {
            var obj = Substitute.For<IActivatable>();
            obj.InitParams.Returns(Unit.Default);
            obj.SaveInitParams.Returns(true);
            var state = await ReActivator.SuspendAsync(obj);
            state.Params.Type.Should().BeAssignableTo<IActivatable>();
            state.Params.Params.Should().Be(Unit.Default);
            state.State.Should().BeNull();
        }

        [Fact]
        public async Task Test_SuspendAsync_Does_Not_Call_DeactivateAsync_On_Activator_For_IActivatable_Objects()
        {
            var obj = Substitute.For<IActivatable>();
            obj.InitParams.Returns(Unit.Default);
            await ReActivator.SuspendAsync(obj);

            Activator.DidNotReceive().DeactivateAsync(obj);
        }

        [Fact]
        public async Task Test_SuspendAsync_Retrieves_InitParams_From_Object_When_SaveInitParams_Is_True()
        {
            var obj = Substitute.For<IReActivatable>();
            var objState = new object();
            obj.InitParams.Returns(Unit.Default);
            obj.SaveInitParams.Returns(true);
            obj.SuspendAsync().Returns(objState);
            var state = await ReActivator.SuspendAsync(obj);

            state.Params.Type.Should().BeAssignableTo<IActivatable>();
            state.Params.Params.Should().Be(Unit.Default);
            state.State.Should().Be(objState);
        }

        [Fact]
        public async Task Test_SuspendAsync_Skips_InitParams_From_Object_When_SaveInitParams_Is_True()
        {
            var obj = Substitute.For<IReActivatable>();
            var objState = new object();
            obj.InitParams.Returns(Unit.Default);
            obj.SuspendAsync().Returns(objState);
            var state = await ReActivator.SuspendAsync(obj);

            state.Params.Type.Should().BeAssignableTo<IActivatable>();
            state.Params.Params.Should().Be(null);
            state.State.Should().Be(objState);
        }

        [Fact]
        public async Task Test_ResumeAsync_Calls_ActivateAsync_On_Activator()
        {
            var state = new ObjectState()
            {
                Params = new ActivationParams()
                {
                    Type = typeof(object),
                    Params = null
                },
                State = null
            };
            var returned = new object();
            Activator.ActivateAsync(state.Params).Returns(returned);
            var resumed = await ReActivator.ResumeAsync(state);

            resumed.Should().Be(returned);
        }

        [Fact]
        public async Task Test_ResumeAsync_Calls_ResumeAsync_On_Activated_Object()
        {
            var state = new ObjectState()
            {
                Params = new ActivationParams(),
                State = new object()
            };
            var reactivatable = Substitute.For<IReActivatable>();
            Activator.ActivateAsync(state.Params).Returns(reactivatable);
            await ReActivator.ResumeAsync(state);

            reactivatable.Received(1).ResumeAsync(state.State, ReActivator);
        }

        [Fact]
        public async Task Test_ResumeAsync_Does_Not_Call_ResumeAsync_On_Activated_Object_When_Stored_State_Is_Null()
        {
            var state = new ObjectState()
            {
                Params = new ActivationParams(),
                State = null
            };
            var reactivatable = Substitute.For<IReActivatable>();
            Activator.ActivateAsync(state.Params).Returns(reactivatable);
            await ReActivator.ResumeAsync(state);

            reactivatable.DidNotReceive().ResumeAsync(state.State, ReActivator);
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
