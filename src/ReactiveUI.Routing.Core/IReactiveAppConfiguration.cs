using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that help configure <see cref="IReactiveApp"/> objects.
    /// </summary>
    public interface IReactiveAppConfiguration
    {
        /// <summary>
        /// Configures the given application.
        /// </summary>
        /// <param name="application">The application that should be configured.</param>
        void Configure(IReactiveApp application);
    }
}
