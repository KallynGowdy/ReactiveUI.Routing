namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a reactive application builder.
    /// That is, an object which helps compile a list of dependencies needed to get an application running.
    /// </summary>
    public interface IReactiveAppBuilder : IReactiveAppDependency
    {
        /// <summary>
        /// Adds the given dependency to the app.
        /// </summary>
        /// <param name="dependency">The dependency.</param>
        IReactiveAppBuilder Add(IReactiveAppDependency dependency);
    }
}