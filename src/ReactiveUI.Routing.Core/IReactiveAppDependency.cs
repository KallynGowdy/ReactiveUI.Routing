using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a dependency for a reactive app.
    /// </summary>
    public interface IReactiveAppDependency
    {
        /// <summary>
        /// Applies the dependency to the given mutable dependency resolver.
        /// </summary>
        /// <param name="resolver">The dependency resolver that the dependencies should be applied to.</param>
        /// <returns></returns>
        void Apply(IMutableDependencyResolver resolver);
    }
}