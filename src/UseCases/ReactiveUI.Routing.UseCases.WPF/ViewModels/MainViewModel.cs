using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Disposables;
using System.Runtime.Serialization;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    [DataContract]
    public class MainViewModel : ReactiveObject, ISupportsActivation
    {
        [IgnoreDataMember]
        private readonly IAppPresenter presenter;
        //private readonly IRouter router;

        [IgnoreDataMember]
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

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

    }
}
