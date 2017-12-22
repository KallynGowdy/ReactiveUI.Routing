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

        /// <summary>
        /// Adds the given configuration to the app.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        IReactiveAppBuilder Configure(IReactiveAppConfiguration configuration);

        /// <summary>
        /// Builds a new reactive app using the configuration defined by the builder.
        /// </summary>
        /// <returns></returns>
        IReactiveApp Build();
    }
}