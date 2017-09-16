using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    [DataContract]
    public class MainViewModel : ReactiveObject, ISupportsActivation
    {
        [IgnoreDataMember]
        private readonly IAppPresenter presenter;

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
