using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationDemos.ViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace PresentationDemos.Pages
{
    public partial class LoginPage : ContentPage, IViewFor<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, vm => vm.Login, view => view.LoginButton));
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LoginViewModel) value; }
        }

        public LoginViewModel ViewModel { get; set; }
    }
}
