using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace ReactiveUI.Routing.UWP
{
    /// <summary>
    /// Extension methods for <see cref="IReactiveAppBuilder"/> objects.
    /// </summary>
    public static class ReactiveAppBuilderExtensions
    {
        public static IReactiveAppBuilder ConfigureUwp(this IReactiveAppBuilder builder, Application application, IActivatedEventArgs onLaunchedEventArgs)
        {
            return builder.Configure(new UwpConfiguration(application, onLaunchedEventArgs));
        }
    }
}
