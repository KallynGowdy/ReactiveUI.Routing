using System.Runtime.Serialization;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing
{
    [DataContract]
    public class ReactiveAppState
    {
        [DataMember]
        public AppPresentationState PresentationState { get; set; }

    }
}
