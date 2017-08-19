using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Hosting;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Routing.UseCases.WPF.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ApplicationViewModel application;

        public App()
        {
            application = new ApplicationViewModel();
            application.Initialize();
            RegisterViews();
            InitializeComponent();
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register<IViewFor<LoginViewModel>>(() => new LoginPage());
            Locator.CurrentMutable.Register<IViewFor<ContentViewModel>>(() => new ContentPage());
        }
    }
}
