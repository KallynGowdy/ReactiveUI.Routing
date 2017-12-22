using System.Runtime.Serialization;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a class that represents a <see cref="NavigationRequest"/> that may or may not have been actively presented
    /// when the app was saved.
    /// </summary>
    [DataContract]
    public class SavedNavigationRequest
    {
        /// <summary>
        /// The navigation request.
        /// </summary>
        [DataMember]
        public NavigationRequest Request { get; set; }

        /// <summary>
        /// Whether the request was actively being presented.
        /// </summary>
        [DataMember]
        public bool Presented { get; set; }
    }
}