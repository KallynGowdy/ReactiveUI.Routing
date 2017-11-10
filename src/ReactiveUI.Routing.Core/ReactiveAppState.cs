using System.Runtime.Serialization;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that contains saved application state.
    /// </summary>
    [DataContract]
    public class ReactiveAppState
    {
        /// <summary>
        /// The container that represents what data was being presented by the application.
        /// </summary>
        [DataMember]
        public AppPresentationState PresentationState { get; set; }
    }
}
