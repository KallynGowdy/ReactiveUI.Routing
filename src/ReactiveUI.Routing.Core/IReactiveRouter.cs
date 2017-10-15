using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a reactive router. 
    /// That is, an object which can keep track of not only which views and view models are currently
    /// being presented, but also the history of navigation between those views and view models.
    /// </summary>
    public interface IReactiveRouter
    {
        /// <summary>
        /// Defines an interaction for when the router wants a piece of content to be presented.
        /// </summary>
        Interaction<PresenterRequest, PresenterResponse> PresentationRequested { get; }

        /// <summary>
        /// Gets the list of stack frames that the router currently posseses.
        /// </summary>
        IEnumerable<NavigationRequest> NavigationStack { get; }

        /// <summary>
        /// Attempts to navigate using the given navigation request.
        /// </summary>
        /// <param name="request">The request that should be used to navigate.</param>
        /// <returns></returns>
        IObservable<Unit> Navigate(NavigationRequest request);
    }

    /// <summary>
    /// Defines a navigation request that can be used
    /// to represent the act of displaying a new piece of content.
    /// </summary>
    public class NavigationRequest
    {
        /// <summary>
        /// Gets the request for the presenter.
        /// This contains the view model that should be presented
        /// and communicate which view should be used.
        /// </summary>
        public PresenterRequest PresenterRequest { get; private set; }
        
        protected NavigationRequest()
        {
        }

        /// <summary>
        /// Creates a new <see cref="NavigationRequest"/> that navigates to the given view model
        /// by adding it to the navigation stack.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static NavigationRequest Forward(object viewModel)
        {
            return new NavigationRequest()
            {
                PresenterRequest = new PagePresenterRequest(viewModel)
            };
        }

        /// <summary>
        /// Creates a new <see cref="NavigationRequest"/> that navigates backwards to the previous view model
        /// by removing the top view model from the stack.
        /// </summary>
        /// <returns></returns>
        public static NavigationRequest Back()
        {
            return new BackNavigationRequest();
        }
    }

    public class BackNavigationRequest : NavigationRequest
    {
    }
}
