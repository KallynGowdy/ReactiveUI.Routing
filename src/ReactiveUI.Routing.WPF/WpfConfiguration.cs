using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Routing.Configuration;
using Splat;

namespace ReactiveUI.Routing.WPF
{
    public class WpfConfiguration : IReactiveAppConfiguration
    {
        private Application application;

        public SuspensionConfiguration Suspension { get; } = new SuspensionConfiguration();

        public WpfConfiguration(Application application)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
        }

        public void Configure(IReactiveApp application)
        {
            AutoSuspendHelper helper = new AutoSuspendHelper(this.application);
            application.Locator.RegisterConstant(helper, typeof(AutoSuspendHelper));
            Suspension.Configure(application);
        }
    }
}
