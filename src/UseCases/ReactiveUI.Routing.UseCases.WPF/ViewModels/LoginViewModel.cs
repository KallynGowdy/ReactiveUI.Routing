using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.WPF.Presenters;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly IAppPresenter presenter;
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