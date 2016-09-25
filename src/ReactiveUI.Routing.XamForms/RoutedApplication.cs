using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReactiveUI.Routing.XamForms
{
    /// <summary>
    /// Defines a class that represents a <see cref="Application"/> that builds a <see cref="IRoutedAppConfig"/> and starts the <see cref="RoutedAppHost"/>.
    /// </summary>
    public abstract class RoutedApplication : Application
    {
        private readonly Lazy<IRoutedAppHost> host;

        protected RoutedApplication()
        {
            host = new Lazy<IRoutedAppHost>(() => new RoutedAppHost(BuildAppConfig()));
        }

        protected abstract IRoutedAppConfig BuildAppConfig();

        protected override void OnSleep()
        {
            host.Value.SaveState();
            base.OnSleep();
        }
        
        protected void StartHost()
        {
            host.Value.Start();
        }
    }
}
