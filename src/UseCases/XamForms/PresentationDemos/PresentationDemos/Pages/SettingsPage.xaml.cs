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
    public partial class SettingsPage : ContentPage, IViewFor<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.MaxTodos, view => view.MaxTodos.Text);
                ViewModel.Load.Execute(null);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SettingsViewModel)value; }
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
