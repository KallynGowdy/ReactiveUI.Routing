using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that bind cross-platform routers and presenters
    /// to their host platform.
    /// </summary>
    public interface IRoutedAppHost
    {
        /// <summary>
        /// Triggers the start of the app host,
        /// which kicks off the router.
        /// </summary>
        void Start();

        /// <summary>
        /// Triggers the start of the app host,
        /// which kicks off the router.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }
}