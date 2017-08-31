using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Extension methods for <see cref="IAppPresenter"/> objects.
    /// </summary>
    public static class AppPresenterExtensions
    {
        /// <summary>
        /// Presents the given request if no views are currently active.
        /// </summary>
        /// <param name="appPresenter">The application presenter.</param>
        /// <param name="request">The presentation request.</param>
        /// <returns>
        /// An observable that resolves with the presenter response. 
        /// If the given request is not presented, then the observable simply completes.
        /// </returns>
        public static IObservable<PresenterResponse> PresentAsDefault(this IAppPresenter appPresenter, Func<PresenterRequest> request)
        {
            if (!appPresenter.ActiveViews.Any())
            {
                return appPresenter.Present(request());
            }
            else
            {
                return Observable.Empty<PresenterResponse>();
            }
        }

        /// <summary>
        /// Presents the given request if no views are currently active.
        /// </summary>
        /// <param name="appPresenter">The application presenter.</param>
        /// <param name="request">The presentation request.</param>
        /// <returns>
        /// An observable that resolves with the presenter response. 
        /// If the given request is not presented, then the observable simply completes.
        /// </returns>
        public static IObservable<PresenterResponse> PresentAsDefault(this IAppPresenter appPresenter, PresenterRequest request)
        {
            return appPresenter.PresentAsDefault(() => request);
        }

        /// <summary>
        /// Presents the given request if no views are currently active.
        /// </summary>
        /// <param name="appPresenter">The application presenter.</param>
        /// <param name="viewModel">The view model that should be presented.</param>
        /// <returns>
        /// An observable that resolves with the presenter response. 
        /// If the given request is not presented, then the observable simply completes.
        /// </returns>
        public static IObservable<PresenterResponse> PresentPageAsDefault(this IAppPresenter appPresenter, Func<object> viewModel)
        {
            return appPresenter.PresentAsDefault(() => new PagePresenterRequest(viewModel()));
        }

        /// <summary>
        /// Presents the given request if no views are currently active.
        /// </summary>
        /// <param name="appPresenter">The application presenter.</param>
        /// <param name="viewModel">The view model that should be presented.</param>
        /// <returns>
        /// An observable that resolves with the presenter response. 
        /// If the given request is not presented, then the observable simply completes.
        /// </returns>
        public static IObservable<PresenterResponse> PresentPageAsDefault(this IAppPresenter appPresenter, object viewModel)
        {
            return appPresenter.PresentPageAsDefault(() => viewModel);
        }
    }
}
