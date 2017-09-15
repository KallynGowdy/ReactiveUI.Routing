using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using ReactiveUI.Routing.Configuration;
using Splat;

namespace ReactiveUI.Routing.UWP
{
    /// <summary>
    /// Defines a configuration that sets up the app for UWP.
    /// </summary>
    public class UwpConfiguration : IReactiveAppConfiguration
    {
        public Application UwpApplication { get; }
        public SuspensionConfiguration Suspension { get; } = new SuspensionConfiguration();

        private readonly IActivatedEventArgs onLaunchedEventArgs;

        public UwpConfiguration(Application uwpApp, IActivatedEventArgs onLaunchedEventArgs)
        {
            this.onLaunchedEventArgs = onLaunchedEventArgs ?? throw new ArgumentNullException(nameof(onLaunchedEventArgs));
            this.UwpApplication = uwpApp ?? throw new ArgumentNullException(nameof(uwpApp));
        }

        public void Configure(IReactiveApp application)
        {
            AutoSuspendHelper helper = new AutoSuspendHelper(UwpApplication);
            application.Locator.RegisterConstant(helper, typeof(AutoSuspendHelper));

            Suspension.Configure(application);

            helper.OnLaunched(onLaunchedEventArgs);
        }
    }
}
