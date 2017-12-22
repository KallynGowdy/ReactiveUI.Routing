using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;

namespace ReactiveUI.Routing.Android
{
    /// <summary>
    /// Extension methods for <see cref="IReactiveAppBuilder"/> objects.
    /// </summary>
    public static class ReactiveAppBuilderExtensions
    {
        public static IReactiveAppBuilder ConfigureAndroid(this IReactiveAppBuilder builder, Application application)
        {
            return builder.Configure(new AndroidConfiguration(application));
        }
    }
}
