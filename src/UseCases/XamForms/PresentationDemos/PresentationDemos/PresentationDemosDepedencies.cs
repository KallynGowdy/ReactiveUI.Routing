using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationDemos.Pages;
using PresentationDemos.Services;
using PresentationDemos.ViewModels;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using Splat;

namespace PresentationDemos
{
    public class PresentationDemosDepedencies : IRegisterDependencies
    {
        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            var routerConfig = new RouterBuilder()
                .Default<LoginViewModel>()
                .When<LoginViewModel>(r => r.Navigate().PresentPage())
                .When<TodoListViewModel>(r => r.NavigateAsRoot().PresentPage())
                .When<SettingsViewModel>(r => r.Navigate().PresentPage())
                .Build();
            resolver.RegisterConstant(routerConfig, typeof(RouterConfig));
            resolver.Register(() => new SettingsService(), typeof(ISettingsService));
            resolver.Register(() => new TodoListViewModel(), typeof(TodoListViewModel));
            resolver.Register(() => new SettingsViewModel(), typeof(SettingsViewModel));
            resolver.Register(() => new LoginViewModel(), typeof(LoginViewModel));
            resolver.Register(() => new TodoListPage(), typeof(TodoListPage));
            resolver.Register(() => new SettingsPage(), typeof(SettingsPage));
            resolver.Register(() => new LoginPage(), typeof(LoginPage));
        }
    }
}
