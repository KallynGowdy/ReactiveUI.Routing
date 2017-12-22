using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveUI.Routing.Configuration
{
    /// <summary>
    /// Defines a <see cref="IReactiveAppConfiguration"/> that uses a given lambda method to configure a <see cref="IReactiveApp"/>.
    /// </summary>
    public class ActionConfiguration : IReactiveAppConfiguration
    {
        private readonly Action<IReactiveApp> action;

        public ActionConfiguration(Action<IReactiveApp> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Configure(IReactiveApp application)
        {
            action(application);
        }
    }
}
