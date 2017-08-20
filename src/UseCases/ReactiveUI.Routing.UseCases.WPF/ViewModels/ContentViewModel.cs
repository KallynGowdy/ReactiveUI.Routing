using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class ContentViewModel
    {
        public string Text { get; set; } = "Hello, Presentation!";

        public ReactiveCommand<Unit, Unit> ShowDetail { get; }

        private IAppPresenter presenter;

        public ContentViewModel(IAppPresenter presenter = null)
        {
            this.presenter = presenter ?? Locator.Current.GetService<IAppPresenter>();
            ShowDetail = ReactiveCommand.CreateFromTask(ShowDetailImpl);
        }

        public async Task ShowDetailImpl()
        {
            await presenter.PresentPage(new DetailViewModel());
        }

    }
}