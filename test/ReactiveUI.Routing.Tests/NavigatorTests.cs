using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class NavigatorTests
    {
        public Navigator Navigator { get; set; }

        public NavigatorTests()
        {
            Navigator = new Navigator();
        }

        [Fact]
        public async Task Test_PushAsync_Notifyies_Subscribers_To_OnTransition()
        {
            var trans = new Transition();
            List<TransitionEvent> transitions = new List<TransitionEvent>();
            Navigator.OnTransition.Subscribe(t => transitions.Add(t));

            await Navigator.PushAsync(trans);

            Assert.Collection(transitions,
                t => t.Current.Should().Be(trans));
        }

        [Fact]
        public async Task Test_PushAsync_Adds_Transition_To_Stack()
        {
            var trans = new Transition();
            
            await Navigator.PushAsync(trans);

            Assert.Collection(Navigator.TransitionStack,
                t => t.Should().Be(trans));
        }

        [Fact]
        public async Task Test_PopAsync_Returns_Removed_Transition()
        {
            var trans = new Transition();
            await Navigator.PushAsync(trans);
            var removed = await Navigator.PopAsync();

            removed.Should().Be(trans);
            Navigator.TransitionStack.Should().BeEmpty();
        }

        [Fact]
        public async Task Test_PopAsync_Notifies_Subscribers_To_OnTransition()
        {
            var trans = new Transition();
            var trans2 = new Transition();
            await Navigator.PushAsync(trans);
            await Navigator.PushAsync(trans2);
            var transitions = new List<TransitionEvent>();
            Navigator.OnTransition.Subscribe(t => transitions.Add(t));
            await Navigator.PopAsync();
            
            Assert.Collection(transitions,
                t => t.Current.Should().Be(trans));
        }

        [Fact]
        public async Task Test_PopAsync_Throws_If_No_Transitions_Are_On_Stack()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Navigator.PopAsync();
            });
        }

        [Fact]
        public void Test_Peek_Returns_Null_If_No_Transitions_Exist_In_Stack()
        {
            Navigator.Peek().Should().BeNull();
        }

        [Fact]
        public async Task Test_Peek_Returns_Topmost_Transition_In_Stack()
        {
            var trans = new Transition();
            var trans2 = new Transition();
            await Navigator.PushAsync(trans);
            await Navigator.PushAsync(trans2);
            Navigator.Peek().Should().Be(trans2);
            await Navigator.PopAsync();
            Navigator.Peek().Should().Be(trans);
        }

        [Fact]
        public async Task Test_SuspendAsync_Returns_NavigatorState_With_TransitionStates()
        {
            var trans = new Transition();
            await Navigator.PushAsync(trans);
            var state = await Navigator.SuspendAsync();

            Assert.Collection(state.TransitionStack,
                t => t.Should().NotBeNull());
        }

        [Fact]
        public async Task Test_ResumeAsync_Populates_TransitionStack()
        {
            var state = new ObjectState()
            {
                Params = new ActivationParams()
                {
                    Params = Unit.Default,
                    Type = typeof(TestViewModel)
                },
                State = null
            };
            var reActivator = Substitute.For<IReActivator>();
            var trans = new Transition();
            reActivator.ResumeAsync(state).Returns(trans);

            await Navigator.ResumeAsync(new NavigatorState()
            {
                TransitionStack = new []
                {
                    state
                }
            }, reActivator);

            Assert.Collection(Navigator.TransitionStack,
                t => t.Should().Be(trans));
        }
    }
}
