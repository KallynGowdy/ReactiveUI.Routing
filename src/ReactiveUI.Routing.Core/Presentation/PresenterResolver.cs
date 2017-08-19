using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

namespace ReactiveUI.Routing.Core.Presentation
{
    public class PresenterResolver
    {
        private readonly List<Func<PresenterRequest, IPresenter>> resolvers = new List<Func<PresenterRequest, IPresenter>>();

        public IPresenter Resolve(PresenterRequest presenterRequest)
        {
            if (presenterRequest == null) throw new ArgumentNullException(nameof(presenterRequest));
            return resolvers.AsEnumerable()
                .Reverse()
                .Select(r => r(presenterRequest))
                .FirstOrDefault(p => p != null);
        }

        public IDisposable Register(Func<PresenterRequest, IPresenter> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            resolvers.Add(predicate);
            return Disposable.Create(() => resolvers.Remove(predicate));
        }

        public IDisposable Register<TRequest>(Func<TRequest, IPresenter> predicate)
            where TRequest : PresenterRequest
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return Register(request => HandleRequest(request, predicate));
        }

        private IPresenter HandleRequest<T>(PresenterRequest request, Func<T, IPresenter> predicate)
            where T : PresenterRequest
        {
            T req = request as T;
            return req != null ? predicate(req) : null;
        }
    }
}
