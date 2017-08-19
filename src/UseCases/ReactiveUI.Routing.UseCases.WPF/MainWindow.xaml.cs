using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI.Routing.Core.Presentation;
using ReactiveUI.Routing.UseCases.WPF.Presenters;
using ReactiveUI.Routing.UseCases.WPF.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            IMutablePresenterResolver resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            
            var disposable = resolver.Register(new PagePresenter(Frame));

            this.WhenActivated(d =>
            {
                d(disposable);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }

        public MainViewModel ViewModel { get; set; }
    }
}
