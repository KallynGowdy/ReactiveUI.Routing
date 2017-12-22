using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a class that contains presentation state for the application.
    /// TODO: Combine with navigation state.
    /// </summary>
    [DataContract]
    public class AppPresentationState
    {
        /// <summary>
        /// The list of presenter requests that were active when the app state was saved.
        /// </summary>
        [DataMember]
        public List<SavedNavigationRequest> PresentationRequests { get; set; }

        public AppPresentationState()
        {
            PresentationRequests = new List<SavedNavigationRequest>();
        }

        public AppPresentationState(IEnumerable<PresentedView> activeViews, IEnumerable<NavigationRequest> navigationStack)
        {
            if (activeViews == null) throw new ArgumentNullException(nameof(activeViews));
            if (navigationStack == null) throw new ArgumentNullException(nameof(navigationStack));

            var requests = from request in navigationStack
                           join view in activeViews on request.PresenterRequest equals view.Request into presentedViews
                           from presented in presentedViews.DefaultIfEmpty()
                           select new SavedNavigationRequest()
                           {
                               Presented = presented != null,
                               Request = request
                           };

            PresentationRequests = requests.ToList();
        }
    }
}
