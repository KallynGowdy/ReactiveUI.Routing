using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace ReactiveUI.Routing
{
    public class DefaultSuspensionHost : ReactiveObject, ISuspensionHost
    {
        public IObservable<Unit> IsLaunchingNew { get; set; }
        public IObservable<Unit> IsResuming { get; set; }
        public IObservable<Unit> IsUnpausing { get; set; }
        public IObservable<IDisposable> ShouldPersistState { get; set; }
        public IObservable<Unit> ShouldInvalidateState { get; set; }
        public Func<object> CreateNewAppState { get; set; }
        public object AppState { get; set; }
    }
}
