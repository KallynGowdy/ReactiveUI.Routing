using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveUI.Routing.Core.Presentation
{
    public abstract class Presenter<TRequest> : IPresenterFor<TRequest>
        where TRequest : PresenterRequest
    {
        public IObservable<PresenterResponse> Present(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return PresentCore((TRequest)request);
        }

        protected abstract IObservable<PresenterResponse> PresentCore(TRequest request);
    }
}
