using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    [DataContract]
    public class LoginViewModel : ReactiveObject
    {
        [IgnoreDataMember]
        private readonly IAppPresenter presenter;

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Login { get; }

        public LoginViewModel(IAppPresenter appPresenter = null)
        {
            this.presenter = appPresenter ?? Locator.Current.GetService<IAppPresenter>();
            Login = ReactiveCommand.CreateFromTask(LoginImpl);
        }

        public async Task<Unit> LoginImpl()
        {
            await presenter.PresentPage(new ContentViewModel());
            return Unit.Default;
        }
    }
}