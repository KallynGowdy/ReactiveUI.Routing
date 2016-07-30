using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that represent an application configuration.
    /// </summary>
    public interface IRoutedAppConfig
    {
        /// <summary>
        /// Registers the dependencies that the app has with the given resolver.
        /// </summary>
        /// <param name="resolver">The resolver that the dependencies should be registered with.</param>
        void RegisterDependencies(IMutableDependencyResolver resolver);

        /// <summary>
        /// Closes the application.
        /// </summary>
        void CloseApp();
    }
}