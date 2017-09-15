using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Hosting;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Routing.UseCases.Common;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ApplicationViewModel application;
        private readonly AutoSuspendHelper suspendHelper;

        public App()
        {
            application = new ApplicationViewModel();
            suspendHelper = new AutoSuspendHelper(this);

            //RxApp.SuspensionHost.WhenAnyValue(h => h.AppState)
            //    .Cast<ReactiveAppState>()
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Do(state => application.LoadState(state))
            //    .Subscribe();
            //RxApp.SuspensionHost.SetupPersistence(() => application.BuildAppState(), new Store<ReactiveAppState>());

            application.Initialize();
            RegisterViews();
            InitializeComponent();
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register<IViewFor<LoginViewModel>>(() => new LoginPage());
            Locator.CurrentMutable.Register<IViewFor<ContentViewModel>>(() => new ContentPage());
            Locator.CurrentMutable.Register<IViewFor<DetailViewModel>>(() => new DetailView());
        }
    }
}
