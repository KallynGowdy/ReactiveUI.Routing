using System;

namespace ReactiveUI.Routing.Presentation
{
    public abstract class Presenter<TRequest> : IPresenterFor<TRequest>
        where TRequest : PresenterRequest
    {
        public IObservable<PresenterResponse> Present(PresenterRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return PresentCore((TRequest)request);
        }

        protected abstract IObservable<PresenterResponse> PresentCore(TRequest request);
    }
}
