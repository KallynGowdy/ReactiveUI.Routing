using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing.Presentation;
using Splat;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests
{
    public class ReactiveAppTests
    {
        public IReactiveApp Subject { get; set; }
        public IReactiveRouter Router { get; set; } = Substitute.For<IReactiveRouter>();
        public IAppPresenter AppPresenter { get; set; } = Substitute.For<IAppPresenter>();
        public ISuspensionHost SuspensionHost { get; set; } = Substitute.For<ISuspensionHost>();
        public ISuspensionDriver SuspensionDriver { get; set; } = Substitute.For<ISuspensionDriver>();
        public IMutableDependencyResolver Resolver { get; set; } = Substitute.For<IMutableDependencyResolver>();

        public ReactiveAppTests()
        {
            Subject = new ReactiveApp(Router, AppPresenter, SuspensionHost, SuspensionDriver, Resolver);
        }

        [Fact]
        public void Test_BuildAppState_Includes_Presenter_State()
        {
            var presenterState = new AppPresentationState();
            AppPresenter.GetPresentationState().Returns(presenterState);

            var state = Subject.BuildAppState();

            Assert.NotNull(state);
            Assert.Same(presenterState, state.PresentationState);
        }

        [Fact]
        public async Task Test_LoadState_Loads_Presenter_State_Into_Presenter()
        {
            var state = new AppPresentationState();
            AppPresenter.LoadState(state).Returns(Observable.Return(Unit.Default));

            await Subject.LoadState(new ReactiveAppState()
            {
                PresentationState = state
            });

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            AppPresenter.Received(1).LoadState(state);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async Task Test_LoadState_Does_Not_Load_Presenter_State_Into_Presenter_When_Presenter_State_Is_Null()
        {
            await Subject.LoadState(new ReactiveAppState()
            {
                PresentationState = null
            });

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            AppPresenter.DidNotReceive().LoadState(Arg.Any<AppPresentationState>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
