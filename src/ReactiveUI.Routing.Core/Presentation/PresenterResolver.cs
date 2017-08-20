using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace ReactiveUI.Routing.Presentation
{
    public class PresenterResolver : IMutablePresenterResolver
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
    }
}
