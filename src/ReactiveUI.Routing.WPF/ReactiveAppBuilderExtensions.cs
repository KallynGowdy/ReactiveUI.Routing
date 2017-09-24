using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Routing.WPF;

namespace ReactiveUI.Routing.Wpf
{
    /// <summary>
    /// Extension methods for <see cref="IReactiveAppBuilder"/> objects.
    /// </summary>
    public static class ReactiveAppBuilderExtensions
    {
        public static IReactiveAppBuilder ConfigureWpf(this IReactiveAppBuilder builder, Application application)
        {
            return builder.Configure(new WpfConfiguration(application));
        }
    }
}
