using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
                d(this.Bind(ViewModel, vm => vm.NewTodo, view => view.NewTodo.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Todos, view => view.Todos.ItemsSource));
                d(this.BindCommand(ViewModel, vm => vm.ViewSettings, view => view.SettingsLink,
                    nameof(ToolbarItem.Clicked)));
                d(Observable.FromEventPattern(h => NewTodo.Completed += h, h => NewTodo.Completed -= h)
                    .InvokeCommand(ViewModel, vm => vm.CreateTodo));
                ViewModel.Load.Execute(null);
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
