using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Builder;
using ShareNavigation.Presenters;
using ShareNavigation.ViewModels;

namespace ShareNavigation
{
    public class RouterConfig
    {

        public IRouter BuildRouter()
        {
            var builder = new RouterBuilder();
            builder.When<PhotoListViewModel>(route => route.Navigate().Present<IPhotoListPresenter>());
            return builder.Build();
        }

    }
}
