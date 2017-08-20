using System;
using Splat;

namespace ReactiveUI.Routing.Presentation
{
    public class AppPresenter : IAppPresenter
    {
        private readonly IMutablePresenterResolver resolver;

        public AppPresenter(IMutablePresenterResolver resolver = null)
        {
            this.resolver = resolver ?? Locator.Current.GetService<IMutablePresenterResolver>();
        }

        public IObservable<PresenterResponse> Present(PresenterRequest request)
        {
            var presenter = resolver.Resolve(request);
            if (presenter != null)
            {
                return presenter.Present(request);
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
    }
}
