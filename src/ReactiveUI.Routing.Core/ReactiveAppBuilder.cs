using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a reactive application builder.
    /// That is, an object which helps compile a list of dependencies needed to get an application running.
    /// </summary>
    public class ReactiveAppBuilder : IReactiveAppBuilder
    {
        private readonly List<IReactiveAppDependency> dependencies = new List<IReactiveAppDependency>();
        private readonly List<IReactiveAppConfiguration> configurations = new List<IReactiveAppConfiguration>();

        public void Apply(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            foreach (var dependency in dependencies)
            {
                dependency.Apply(resolver);
            }
        }

        public IReactiveAppBuilder Add(IReactiveAppDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));
            dependencies.Add(dependency);

            return this;
        }

        public IReactiveAppBuilder Configure(IReactiveAppConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configurations.Add(configuration);

            return this;
        }

        public IReactiveApp Build()
        {
            Apply(Locator.CurrentMutable);

            var app = new ReactiveApp(
                router: Locator.Current.GetService<IReactiveRouter>(),
                presenter: Locator.Current.GetService<IAppPresenter>(),
                suspensionHost: Locator.Current.GetService<ISuspensionHost>() ?? RxApp.SuspensionHost,
                suspensionDriver: Locator.Current.GetService<ISuspensionDriver>(),
                locator: Locator.CurrentMutable);

            foreach (var config in configurations)
            {
                config.Configure(app);
            }

            Locator.CurrentMutable.RegisterConstant(app, typeof(IReactiveApp));
            Locator.CurrentMutable.RegisterConstant(app, typeof(ReactiveApp));

            return app;
        }
    }
}
