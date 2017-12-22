using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Android.Utils;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.Android
{
    public class AndroidViewDependencies : IReactiveAppDependency
    {
        public void Apply(IMutableDependencyResolver resolver)
        {
            resolver.Register<IViewFor<LoginViewModel>>(() => new LoginPage());
            resolver.Register<IViewFor<ContentViewModel>>(() => new ContentPage());
            resolver.Register<IViewFor<DetailViewModel>>(() => new DetailPage());

            resolver.RegisterConstant(new LogcatLogger("ReactiveUI.Routing.UseCases.Android"), typeof(ILogger));
        }
    }
}
