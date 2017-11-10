using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing.Presentation;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests.Presentation
{
    public class AppPresenterTests
    {
        private PresenterRequest request;
        private IViewFor view;
        private ICanActivate activatable;
        private PresenterResponse response;
        private IPresenter presenter;

        public IAppPresenter Subject { get; set; }
        public IPresenterResolver PresenterResolver { get; set; }

        public AppPresenterTests()
        {
            request = new PresenterRequest();
            view = Substitute.For<IViewFor, ICanActivate>();
            activatable = (ICanActivate)view;
            response = new PresenterResponse(view);
            presenter = Substitute.For<IPresenter>();

            activatable.Activated.Returns(Observable.Return(Unit.Default));
            activatable.Deactivated.Returns(Observable.Never<Unit>());

            PresenterResolver = Substitute.For<IPresenterResolver>();
            Subject = new AppPresenter(PresenterResolver);
        }

        [Fact]
        public async Task Test_Present_Throws_ArgumentException_If_No_Presenter_Was_Found()
        {
            PresenterResolver.Resolve(request).Returns(c => null);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Subject.Present(request);
            });
        }

        [Fact]
        public async Task Test_Present_Returns_Presenter_Response_Returned_From_Presenter()
        {
            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            var r = await Subject.Present(request);

            Assert.Same(response, r);
        }

        [Fact]
        public async Task Test_Present_Adds_View_To_Active_Views()
        {
            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            await Subject.Present(request);

            Assert.Collection(Subject.ActiveViews,
                v =>
                {
                    Assert.Same(request, v.Request);
                    Assert.Same(response, v.Response);
                    Assert.Same(presenter, v.Presenter);
                });
        }

        [Fact]
        public async Task Test_Active_View_Is_Updated_When_View_Is_Deactivated()
        {
            var deactivated = new Subject<Unit>();

            activatable.Activated.Returns(Observable.Return(Unit.Default));
            activatable.Deactivated.Returns(deactivated);

            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            await Subject.Present(request);

            deactivated.OnNext(Unit.Default);

            Assert.Empty(Subject.ActiveViews);
        }

        [Fact]
        public void Test_GetPresentationState_Returns_The_Currently_Active_Views()
        {
            var nav = NavigationRequest.Forward(new TestViewModel());
            var presentedView = new PresentedView(response, nav.PresenterRequest, presenter);

            Subject.ActiveViews.Add(presentedView);

            var state = new AppPresentationState(Subject.ActiveViews, new[]
            {
                nav
            });

            Assert.NotNull(state);
            Assert.Collection(state.PresentationRequests,
                r =>
                {
                    Assert.True(r.Presented);
                    Assert.Same(nav, r.Request);
                });
        }

        [Fact]
        public async Task Test_PresentAsDefault_Presents_Given_Request_If_No_Views_Are_Active()
        {
            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            var r = await Subject.PresentAsDefault(request);

            Assert.Same(response, r);
        }

        [Fact]
        public void Test_PresentAsDefault_Does_Not_Present_Given_Request_If_At_Least_One_View_Is_Active()
        {
            var presentedView = new PresentedView(response, request, presenter);
            var resolved = false;
            var completed = false;
            var called = false;

            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            Subject.ActiveViews.Add(presentedView);

            Subject.PresentAsDefault(() =>
                {
                    called = true;
                    return request;
                })
                .Subscribe(u => resolved = true, () => completed = true);

            Assert.False(called, "Should not have called request func.");
            Assert.False(resolved, "Should not have resolved with a value.");
            Assert.True(completed, "Should have completed the returned observable.");
        }

        [Fact]
        public async Task Test_PresentPageAsDefault_Presents_Given_ViewModel_If_No_Views_Are_Active()
        {
            var viewModel = new object();

            PresenterResolver.Resolve(Arg.Is<PagePresenterRequest>(r => r.ViewModel == viewModel)).Returns(presenter);
            presenter.Present(Arg.Any<PagePresenterRequest>()).Returns(Observable.Return(response));

            var result = await Subject.PresentPageAsDefault(viewModel);

            Assert.Same(response, result);
        }

        [Fact]
        public async Task Test_LoadState_Replays_The_Presentation_Requests_Contained_In_The_Given_State()
        {
            var nav = NavigationRequest.Forward(new TestViewModel());
            var request = nav.PresenterRequest;
            PresenterResolver.Resolve(request).Returns(presenter);
            presenter.Present(request).Returns(Observable.Return(response));

            await Subject.LoadState(new AppPresentationState()
            {
                PresentationRequests = new List<SavedNavigationRequest>()
                {
                    new SavedNavigationRequest()
                    {
                        Presented = true,
                        Request = nav
                    },
                    new SavedNavigationRequest()
                    {
                        Presented = true,
                        Request = nav
                    }
                }
            });

            Assert.Collection(Subject.ActiveViews,
                v => Assert.Same(response, v.Response),
                v => Assert.Same(response, v.Response));
        }
    }
}
