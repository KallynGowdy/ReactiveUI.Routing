using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class DetailViewModel : ReactiveObject
    {
        private IAppPresenter presenter;

        public ReactiveCommand<Unit, Unit> BackToLogin { get; }

        public DetailViewModel(IAppPresenter presenter = null)
        {
            this.presenter = presenter ?? Locator.Current.GetService<IAppPresenter>();

            BackToLogin = ReactiveCommand.CreateFromTask(BackToLoginImpl);
        }

        private async Task BackToLoginImpl()
        {
            await presenter.PresentPage(new LoginViewModel());
        }
    }
}
