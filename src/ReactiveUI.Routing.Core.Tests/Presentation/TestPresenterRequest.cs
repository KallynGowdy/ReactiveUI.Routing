using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing.Core.Tests.Presentation
{
    public class TestPresenterRequest : PresenterRequest
    {
        public TestPresenterRequest() { }

        public TestPresenterRequest(object viewModel)
            : base(viewModel)
        {
        }
    }
}