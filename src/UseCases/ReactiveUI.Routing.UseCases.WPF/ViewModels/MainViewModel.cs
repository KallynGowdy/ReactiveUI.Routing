using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly IAppPresenter presenter;
        //private readonly IRouter router;

        public MainViewModel(IAppPresenter presenter = null)
        {
            this.presenter = presenter ?? Locator.Current.GetService<IAppPresenter>();

            this.WhenActivated(d =>
            {
                this.presenter.PresentPage(new LoginViewModel())
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
