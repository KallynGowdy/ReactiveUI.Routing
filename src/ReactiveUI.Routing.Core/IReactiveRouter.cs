using System;
using System.Collections;
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
        /// Gets or sets the list of stack frames that the router currently posseses.
        /// </summary>
        IEnumerable<NavigationRequest> NavigationStack { get; set; }

        /// <summary>
        /// Attempts to navigate using the given navigation request.
        /// </summary>
        /// <param name="request">The request that should be used to navigate.</param>
        /// <returns></returns>
        IObservable<Unit> Navigate(NavigationRequest request);
        
        /// <summary>
        /// Calculates an observable that determines whether the given navigation request is executable.
        /// The observable will immediately resolve with a value upon subscription,
        /// but may also continue to live and update the value as the router state changes over time.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IObservable<bool> CanNavigate(NavigationRequest request);
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

        public static CombinedNavigationRequest operator +(NavigationRequest first, NavigationRequest second)
        {
            return new CombinedNavigationRequest(first, second);
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

        /// <summary>
        /// Creates a new <see cref="NavigationRequest"/> that resets the navigation stack, leaving nothing.
        /// </summary>
        /// <returns></returns>
        public static NavigationRequest Reset()
        {
            return new ResetNavigationRequest();
        }
    }

    public class CombinedNavigationRequest : NavigationRequest, IEnumerable<NavigationRequest>
    {
        private List<NavigationRequest> requests = new List<NavigationRequest>(2);

        public CombinedNavigationRequest(NavigationRequest first, NavigationRequest second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            Add(first);
            Add(second);
        }

        public void Add(NavigationRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request is CombinedNavigationRequest combined)
            {
                requests.AddRange(combined.requests);
            }
            else
            {
                requests.Add(request);
            }
        }

        public IEnumerator<NavigationRequest> GetEnumerator() => requests.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ResetNavigationRequest : NavigationRequest
    {
    }

    public class BackNavigationRequest : NavigationRequest
    {
    }
}
