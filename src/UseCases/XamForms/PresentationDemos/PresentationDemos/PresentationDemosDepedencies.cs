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
                .Default<TodoListViewModel>()
                .When<TodoListViewModel>(r => r.Navigate().PresentPage())
                .When<SettingsViewModel>(r => r.Navigate().PresentPage())
                .Build();
            resolver.RegisterConstant(routerConfig, typeof(RouterConfig));
            resolver.Register(() => new SettingsService(), typeof(ISettingsService));
            resolver.Register(() => new TodoListViewModel(), typeof(TodoListViewModel));
            resolver.Register(() => new SettingsViewModel(), typeof(SettingsViewModel));
            resolver.Register(() => new TodoListPage(), typeof(TodoListPage));
            resolver.Register(() => new SettingsPage(), typeof(SettingsPage));
        }
    }
}
