using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a <see cref="IRoutedAppConfig"/> that combines multiple other configurations into one.
    /// </summary>
    public class CompositeRoutedAppConfig : IRoutedAppConfig
    {
        private readonly IRegisterDependencies[] configs;

        public CompositeRoutedAppConfig(params IRegisterDependencies[] configs)
        {
            if (configs == null) throw new ArgumentNullException(nameof(configs));
            this.configs = configs.Where(c => c != null).ToArray();
            if (GetCloseAppConfig() == null)
            {
                throw new ArgumentException($"At least one of the given configs must implement {nameof(IRoutedAppConfig)}", nameof(configs));
            }
        }

        public virtual void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            foreach (var c in configs)
            {
                c.RegisterDependencies(resolver);
            }
        }

        public void CloseApp()
        {
            var c = GetCloseAppConfig();
            if (c != null)
                c.CloseApp();
            else
            {
                throw new InvalidOperationException($"Cannot close application because no sub configurations were registered.");
            }
        }

        private IRoutedAppConfig GetCloseAppConfig()
        {
            var c = configs.OfType<IRoutedAppConfig>().LastOrDefault();
            return c;
        }
    }
}
