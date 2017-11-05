﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using NSubstitute;
using ReactiveUI.Routing.Core.Tests.Presentation;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Testing;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests
{
    public class ReactiveRouterTests : IDisposable
    {
        public IReactiveRouter Subject { get; set; }
        public PresenterResponse PresenterResponse { get; set; }

        private CompositeDisposable disposable = new CompositeDisposable();

        public ReactiveRouterTests()
        {
            Subject = new ReactiveRouter();

            Subject.PresentationRequested.RegisterHandler(x => x.SetOutput(PresenterResponse))
                .DisposeWith(disposable);
        }

        [Fact]
        public async Task Test_Navigate_Adds_ViewModel_To_Navigation_Stack_By_Default()
        {
            var viewModel = new TestViewModel();
            var request = NavigationRequest.Forward(viewModel);
            await Subject.Navigate(request);

            Assert.Collection(Subject.NavigationStack,
                frame => Assert.Same(viewModel, frame.PresenterRequest.ViewModel));
        }

        [Fact]
        public async Task Test_Navigate_Adds_PresentationRequest_To_Navigation_Stack()
        {
            var viewModel = new TestViewModel();
            var request = NavigationRequest.Forward(viewModel);
            await Subject.Navigate(request);

            Assert.Collection(Subject.NavigationStack,
                frame => Assert.Same(request.PresenterRequest, frame.PresenterRequest));
        }

        [Fact]
        public async Task Test_Navigate_Back_Replays_Previous_Presentation_Request()
        {
            var request = NavigationRequest.Forward(new TestViewModel());
            await Subject.Navigate(request);
            await Subject.Navigate(NavigationRequest.Forward(new TestViewModel()));

            var back = NavigationRequest.Back();

            using (Subject.PresentationRequested.RegisterHandler(x =>
            {
                Assert.Same(request.PresenterRequest, x.Input);
                x.SetOutput(new PresenterResponse(Substitute.For<IViewFor>()));
            }))
            {
                await Subject.Navigate(back);
            }
        }

        [Fact]
        public async Task Test_Does_Not_Add_ViewModel_To_Navigation_Stack_If_Presentation_Fails()
        {
            using (Subject.PresentationRequested.RegisterHandler(x =>
            {
                throw new InvalidOperationException();
            }))
            {
                var viewModel = new TestViewModel();
                var request = NavigationRequest.Forward(viewModel);
                try
                {
                    await Subject.Navigate(request);
                }
                catch (InvalidOperationException)
                {
                }

                Assert.Empty(Subject.NavigationStack);
            }
        }

        [Fact]
        public async Task Test_Navigate_Uses_Interactions_To_Notify_Of_Presentation()
        {
            var test = new TestViewModel();
            var request = NavigationRequest.Forward(test);
            using (Subject.PresentationRequested.RegisterHandler(x =>
            {
                Assert.Same(request.PresenterRequest, x.Input);
                x.SetOutput(new PresenterResponse(Substitute.For<IViewFor>()));
            }))
            {
                await Subject.Navigate(request);
            }
        }

        [Fact]
        public async Task Test_Navigate_Throws_Exception_When_Navigation_Is_Not_Allowed()
        {
            var back = NavigationRequest.Back();
            InvalidOperationException e = null;

            try
            {
                await Subject.Navigate(back);
            }
            catch (InvalidOperationException ex)
            {
                e = ex;
            }

            Assert.NotNull(e);
            Assert.Equal("Cannot perform the given navigation request because it would result in an invalid state.", e.Message);
        }

        [Fact]
        public async Task Test_Reset_Clears_The_Stack()
        {
            var test = new TestViewModel();
            var request = NavigationRequest.Reset();
            using (Subject.PresentationRequested.RegisterHandler(x =>
            {
                x.SetOutput(new PresenterResponse(Substitute.For<IViewFor>()));
            }))
            {
                await Subject.Navigate(NavigationRequest.Forward(test));
                await Subject.Navigate(request);
                Assert.Empty(Subject.NavigationStack);
            }
        }

        [Fact]
        public async Task Test_Navigate_And_Reset_Leaves_Only_The_New_Response_On_The_Stack()
        {
            var test = new TestViewModel();
            var forward = NavigationRequest.Forward(test);
            var request = NavigationRequest.Reset() + forward;
            using (Subject.PresentationRequested.RegisterHandler(x =>
            {
                x.SetOutput(new PresenterResponse(Substitute.For<IViewFor>()));
            }))
            {
                await Subject.Navigate(request);

                Assert.Collection(Subject.NavigationStack,
                    x => Assert.Same(forward, x));
            }
        }

        [Fact]
        public async Task Test_CanNavigate_Returns_False_When_NavigationStack_Is_Empty()
        {
            var back = NavigationRequest.Back();

            bool canNavigate = await Subject.CanNavigate(back).FirstAsync();

            Assert.False(canNavigate);
        }

        [Fact]
        public async Task Test_CanNavigate_Returns_False_When_NavigationStack_Only_Has_A_Single_ViewModel()
        {
            var back = NavigationRequest.Back();
            var observable = Subject.CanNavigate(back);
            var results = new List<bool>();

            using (observable.Do(x => results.Add(x)).Subscribe())
            {
                await Subject.Navigate(NavigationRequest.Forward(new TestViewModel()));

                Assert.Collection(results,
                    Assert.False,
                    Assert.False);
            }
        }

        [Fact]
        public async Task Test_CanNavigate_Returns_True_When_NavigationStack_Is_Not_Empty()
        {
            var back = NavigationRequest.Back();
            var observable = Subject.CanNavigate(back);
            var results = new List<bool>();

            using (observable.Do(x => results.Add(x)).Subscribe())
            {
                await Subject.Navigate(NavigationRequest.Forward(new TestViewModel()));
                await Subject.Navigate(NavigationRequest.Forward(new TestViewModel()));
                await Subject.Navigate(back);

                Assert.Collection(results,
                    Assert.False,
                    Assert.False,
                    Assert.True,
                    Assert.False);
            }
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
