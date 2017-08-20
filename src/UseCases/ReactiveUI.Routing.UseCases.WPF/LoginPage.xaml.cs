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
using ReactiveUI.Routing.UseCases.WPF.ViewModels;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, IViewFor<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.Login, view => view.Login)
                    .DisposeWith(d);

                Disposable.Create(() =>
                {
                    Console.WriteLine("Disposed");
                }).DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LoginViewModel) value;
        }

        public LoginViewModel ViewModel { get; set; }
    }
}
