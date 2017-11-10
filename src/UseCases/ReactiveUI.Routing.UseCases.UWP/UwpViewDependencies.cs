using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using ReactiveUI.Routing.UWP;

namespace ReactiveUI.Routing.UseCases.UWP
{
    public class UwpViewDependencies : ReactiveAppBuilder
    {
        public UwpViewDependencies()
        {
            this.Register(() => new MainPage(), typeof(IViewFor<MainViewModel>));
            this.Register(() => new LoginPage(), typeof(IViewFor<LoginViewModel>));
            this.Register(() => new ContentPage(), typeof(IViewFor<ContentViewModel>));
            this.Register(() => new DetailPage(), typeof(IViewFor<DetailViewModel>));
        }
    }
}
