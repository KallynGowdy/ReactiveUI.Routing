using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI.Routing.Core.Presentation;
using ReactiveUI.Routing.Core.Routing;
using ReactiveUI.Routing.UseCases.WPF.Presenters;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly IMutablePresenterResolver presenter;
        //private readonly IRouter router;

        public MainViewModel(IMutablePresenterResolver presenter = null)
        {
            this.presenter = presenter ?? Locator.Current.GetService<IMutablePresenterResolver>();

            this.WhenActivated(d =>
            {
                var request = new PagePresenterRequest(new LoginViewModel());
                var p = this.presenter.Resolve(request);
                p.Present(request)
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
