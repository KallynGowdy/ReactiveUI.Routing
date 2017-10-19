using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    [DataContract]
    public class DetailViewModel : ReactiveObject
    {
        [IgnoreDataMember]
        private IReactiveRouter router;

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> BackToLogin { get; }

        public DetailViewModel(IReactiveRouter router = null)
        {
            this.router = router ?? Locator.Current.GetService<IReactiveRouter>();

            BackToLogin = ReactiveCommand.CreateFromTask(BackToLoginImpl);
        }

        private async Task BackToLoginImpl()
        {
            var request = NavigationRequest.Reset() + NavigationRequest.Forward(new LoginViewModel());
            await router.Navigate(request);
        }
    }
}
