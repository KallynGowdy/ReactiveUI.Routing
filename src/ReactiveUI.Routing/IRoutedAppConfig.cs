using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that represent an application configuration.
    /// </summary>
    public interface IRoutedAppConfig
    {
        void RegisterDependencies(IMutableDependencyResolver resolver);
    }
}