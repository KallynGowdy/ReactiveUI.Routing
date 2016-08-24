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
    public partial class TodoListPage : ContentPage, IViewFor<TodoListViewModel>
    {
        public TodoListPage()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Todos, view => view.Todos.ItemsSource));
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TodoListViewModel)value; }
        }

        public TodoListViewModel ViewModel { get; set; }
    }
}
