using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that contains common core functionality for routed apps.
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public class RoutedAppHost : IRoutedAppHost
    {
        public IRoutedAppConfig Config { get; }

        public RoutedAppHost(IRoutedAppConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            Config = config;
        }

        public void Start()
        {
            Task.Run(StartAsync).Wait();
        }

        public virtual async Task StartAsync()
        {
            Config.RegisterDependencies(Locator.CurrentMutable);
            await Config.BuildRouterAsync();
        }
    }
}
