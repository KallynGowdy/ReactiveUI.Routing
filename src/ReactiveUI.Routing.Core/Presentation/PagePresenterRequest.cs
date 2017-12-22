using System.Runtime.Serialization;

namespace ReactiveUI.Routing.Presentation
{
    [DataContract]
    public class PagePresenterRequest : PresenterRequest
    {
        public PagePresenterRequest()
        {
        }

        public PagePresenterRequest(object viewModel) : base(viewModel)
        {
        }
    }
}
