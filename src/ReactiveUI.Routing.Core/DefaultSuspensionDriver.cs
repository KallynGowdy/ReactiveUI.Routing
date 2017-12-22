using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace ReactiveUI.Routing
{
    public class DefaultSuspensionDriver : ISuspensionDriver
    {
        public IObservable<object> LoadState()
        {
            return Observable.Return((object)null);
        }

        public IObservable<Unit> SaveState(object state)
        {
            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> InvalidateState()
        {
            return Observable.Return(Unit.Default);
        }
    }
}
