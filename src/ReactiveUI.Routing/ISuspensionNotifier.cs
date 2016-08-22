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
        /// Gets an observable that resolves when the app should save its state.
        /// This is usually means that the app could be terminated at any moment without notice,
        /// but may not be terminated.
        /// </summary>
        IObservable<Unit> OnSaveState { get; }

        /// <summary>
        /// Gets an observable that resolves when the app is being closed.
        /// Usually resolved in combination with OnSaveState.
        /// </summary>
        IObservable<Unit> OnSuspend { get; }
    }
}
