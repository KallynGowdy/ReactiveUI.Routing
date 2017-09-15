using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace ReactiveUI.Routing
{
    public class DefaultSuspensionDriver : ISuspensionDriver
    {
        public IObservable<object> LoadState()
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SaveState(object state)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> InvalidateState()
        {
            throw new NotImplementedException();
        }
    }
}
