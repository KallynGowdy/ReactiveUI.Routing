using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that register dependencies in the application.
    /// </summary>
    public interface IRegisterDependencies
    {
        /// <summary>
        /// Registers the dependencies that the app has with the given resolver.
        /// </summary>
        /// <param name="resolver">The resolver that the dependencies should be registered with.</param>
        void RegisterDependencies(IMutableDependencyResolver resolver);
    }
}