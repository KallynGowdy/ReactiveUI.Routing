using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using ReactiveUI.Routing.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ReactiveUI.Routing.UseCases.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page, IViewFor<ContentViewModel>
    {
        public ContentPage()
        {
            this.InitializeComponent();
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.ShowDetail, view => view.ShowDetailButton)
                    .DisposeWith(d);

                PagePresenter.RegisterHostFor<DetailViewModel>(Detail)
                    .DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ContentViewModel)value; }
        }

        public ContentViewModel ViewModel { get; set; }
    }
}
