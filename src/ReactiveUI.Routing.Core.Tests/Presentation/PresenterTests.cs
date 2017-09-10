using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing.Presentation;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests.Presentation
{
    public class PresenterTests
    {
        private class TestPresenter : Presenter<TestPresenterRequest>
        {
            public PresenterResponse Response { get; set; } = new PresenterResponse(Substitute.For<IViewFor>());

            protected override IObservable<PresenterResponse> PresentCore(TestPresenterRequest request)
            {
                return Observable.Return(Response);
            }
        }

        public Presenter<TestPresenterRequest> Subject { get; set; }

        public PresenterTests()
        {
            Subject = new TestPresenter();
        }

        [Fact]
        public void Test_Present_Checks_For_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Subject.Present(null);
            });
        }

        [Fact]
        public async Task Test_Checks_For_Correct_Presenter_Request_Type()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Subject.Present(new PresenterRequest());
            });
        }

        [Fact]
        public async Task Test_Calls_PresentCore()
        {
            TestPresenter presenter = new TestPresenter();

            var result = await presenter.Present(new TestPresenterRequest());

            Assert.Same(presenter.Response, result);
        }
    }
}
