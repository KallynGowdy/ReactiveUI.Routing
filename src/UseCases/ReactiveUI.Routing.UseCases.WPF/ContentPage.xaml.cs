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
using ReactiveUI.Routing.UseCases.WPF.Presenters;
using ReactiveUI.Routing.UseCases.WPF.ViewModels;

namespace ReactiveUI.Routing.UseCases.WPF
{
    /// <summary>
    /// Interaction logic for ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page, IViewFor<ContentViewModel>
    {
        public ContentPage()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                PagePresenter.RegisterHostFor<DetailViewModel>(Detail)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Text, view => view.Text.Text)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.ShowDetail, view => view.DisplayDetail);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ContentViewModel) value;
        }

        public ContentViewModel ViewModel { get; set; }
    }
}
