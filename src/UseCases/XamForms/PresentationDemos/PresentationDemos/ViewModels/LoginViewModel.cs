using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace PresentationDemos.ViewModels
{
    public class LoginViewModel : ReActivatableObject<Unit, Unit>
    {
        public LoginViewModel(IRouter router = null)
        {
            Router = router ?? Locator.Current.GetService<IRouter>();
            Login = ReactiveCommand.CreateFromTask(async o => await Router.ShowAsync<TodoListViewModel>());
        }

        public IRouter Router { get; set; }
        public ReactiveCommand<Unit, Unit> Login { get; }
    }
}
