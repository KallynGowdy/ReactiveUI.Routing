using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a class that contains presentation state for the application.
    /// </summary>
    [DataContract]
    public class AppPresentationState
    {
        [DataMember]
        public List<PresenterRequest> PresentationRequests { get; set; }

        public AppPresentationState()
        {
            PresentationRequests = new List<PresenterRequest>();
        }

        public AppPresentationState(IEnumerable<PresentedView> views)
        {
            PresentationRequests = views.Select(v => v.Request).ToList();
        }
    }
}
