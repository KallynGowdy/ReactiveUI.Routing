using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for an object that can notifiy a routed application that it should suspend itself.
    /// </summary>
    public interface ISuspensionNotifier
    {
        /// <summary>
        /// Gets an observable that resolves when the app should be suspended.
        /// </summary>
        IObservable<Unit> OnSuspend { get; }
    }
}
