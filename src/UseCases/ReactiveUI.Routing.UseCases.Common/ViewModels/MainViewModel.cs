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
        private readonly IReactiveRouter router;

        [IgnoreDataMember]
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public MainViewModel(IReactiveRouter router = null)
        {
            this.router = router ?? Locator.Current.GetService<IReactiveRouter>();

            this.WhenActivated(d =>
            {
                this.router.Navigate(NavigationRequest.Forward(new LoginViewModel()))
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

    }
}
