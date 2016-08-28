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
    public abstract class RoutedApplication : Application, ISuspensionNotifier
    {
        private readonly Subject<Unit> onSaveState = new Subject<Unit>();
        private readonly Subject<Unit> onSuspend = new Subject<Unit>();

        // Needed because a new Xamarin.Forms application will be created
        // each time that the application is re-entered
        private Lazy<IRoutedAppHost> host;
        public int Id { get; set; }
        private static int c;
        protected RoutedApplication()
        {
            host = new Lazy<IRoutedAppHost>(() => new RoutedAppHost(BuildAppConfig()));
            Id = c++;
        }

        protected abstract IRoutedAppConfig BuildAppConfig();

        protected override void OnSleep()
        {
            onSaveState.OnNext(Unit.Default);
            base.OnSleep();
            onSuspend.OnNext(Unit.Default);
        }

        protected override void OnResume()
        {
            base.OnResume();
            
        }

        protected void StartHost()
        {
            host.Value.Start();
        }

        public IObservable<Unit> OnSaveState => onSaveState;
        public IObservable<Unit> OnSuspend => onSuspend;
    }
}
