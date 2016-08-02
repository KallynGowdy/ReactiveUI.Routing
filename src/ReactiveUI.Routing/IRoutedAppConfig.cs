using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that represent an application configuration.
    /// </summary>
    public interface IRoutedAppConfig : IRegisterDependencies
    {
        /// <summary>
        /// Closes the application.
        /// </summary>
        void CloseApp();
    }
}