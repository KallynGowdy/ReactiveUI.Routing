using System.Runtime.Serialization;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing.UseCases.Common
{
    [DataContract]
    public class ReactiveAppState
    {
        [DataMember]
        public AppPresentationState PresentationState { get; set; }

    }
}
