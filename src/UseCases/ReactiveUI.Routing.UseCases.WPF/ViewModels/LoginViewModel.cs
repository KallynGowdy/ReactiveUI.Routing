using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI.Routing.Core.Presentation;
using ReactiveUI.Routing.Core.Routing;
using ReactiveUI.Routing.UseCases.WPF.Presenters;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly IMutablePresenterResolver presenter;
        public ReactiveCommand<Unit, Unit> Login { get; }

        public LoginViewModel(IMutablePresenterResolver appPresenter = null)
        {
            this.presenter = appPresenter ?? Locator.Current.GetService<IMutablePresenterResolver>();
            Login = ReactiveCommand.CreateFromTask(LoginImpl);
        }

        public async Task<Unit> LoginImpl()
        {
            var request = new PagePresenterRequest(new ContentViewModel());
            var p = presenter.Resolve(request);
            await p.Present(request);
            return Unit.Default;
        }
    }
}