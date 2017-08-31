using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Splat;

namespace ReactiveUI.Routing.Presentation
{
    public class AppPresenter : IAppPresenter
    {
        private readonly IPresenterResolver resolver;
        private readonly Subject<PresentedView> presentationResponses = new Subject<PresentedView>();

        public ReactiveList<PresentedView> ActiveViews { get; }

        public AppPresenter(IPresenterResolver resolver = null, IActivationForViewFetcher activationForViewFetcher = null)
        {
            this.resolver = resolver ?? Locator.Current.GetService<IPresenterResolver>();
            ActiveViews = new ReactiveList<PresentedView>();

            this.presentationResponses
                .Do(presented => ActiveViews.Add(presented))
                .SelectMany(presented => presented.Response.PresentedView.Deactivated(activationForViewFetcher).Select(v => presented))
                .Do(view => ActiveViews.Remove(view))
                .Subscribe();
        }

        public IObservable<PresenterResponse> Present(PresenterRequest request)
        {
            var presenter = resolver.Resolve(request);
            if (presenter != null)
            {
                return presenter.Present(request)
                    .Do(response => presentationResponses.OnNext(new PresentedView(response, request, presenter)));
            }
            else
            {
                throw new ArgumentException("No presenter found for the given request", nameof(request));
            }
        }

        public IObservable<PresenterResponse> PresentPage(object viewModel)
        {
            return Present(new PagePresenterRequest(viewModel));
        }

        public AppPresentationState GetPresentationState()
        {
            return new AppPresentationState(ActiveViews);
        }

        public IObservable<Unit> LoadState(AppPresentationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            return Observable.StartAsync(async () =>
            {
                foreach (var observable in state.PresentationRequests.Select(Present))
                {
                    await observable;
                }
            });
        }
    }
}
