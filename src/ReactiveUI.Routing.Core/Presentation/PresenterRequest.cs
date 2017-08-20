using System.Runtime.Serialization;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a class that is used to request presenters from a <see cref="IPresenterResolver"/>.
    /// </summary>
    [DataContract]
    public class PresenterRequest
    {
        [DataMember]
        public object ViewModel { get; set; }

        public PresenterRequest(object viewModel)
        {
            this.ViewModel = viewModel;
        }

        public PresenterRequest()
        {
        }

    }
}
