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
using ReactiveUI.Routing.Wpf;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AutoSuspendHelper suspendHelper;

        public App()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveRouting()
                .Add(new CommonUseCaseDependencies())
                .Add(new WpfViewDependencies())
                .ConfigureWpf(this)
                .Build();
            
            InitializeComponent();
        }
    }
}
