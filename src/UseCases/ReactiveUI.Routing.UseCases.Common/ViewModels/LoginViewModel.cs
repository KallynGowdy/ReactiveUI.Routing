using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    [DataContract]
    public class LoginViewModel : ReactiveObject
    {
        [IgnoreDataMember]
        private readonly IReactiveRouter router;

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Login { get; }

        public LoginViewModel(IReactiveRouter router = null)
        {
            this.router = router ?? Locator.Current.GetService<IReactiveRouter>();
            Login = ReactiveCommand.CreateFromTask(LoginImpl);
        }

        public async Task<Unit> LoginImpl()
        {
            await router.Navigate(NavigationRequest.Forward(new ContentViewModel()));
            return Unit.Default;
        }
    }
}