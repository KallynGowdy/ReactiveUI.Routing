using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class ApplicationViewModel : ReactiveObject
    {
        public IAppPresenter Presenter { get; set; }

        private AppState state;

        public ApplicationViewModel()
        {
        }

        public void LoadState(AppState state)
        {
            this.state = state;
            if (state?.PresentationState != null)
            {
                Presenter.LoadState(state.PresentationState)
                    .Subscribe();
            }
        }

        public void Initialize()
        {
            Locator.CurrentMutable.RegisterRouting();
            Presenter = Locator.Current.GetService<IAppPresenter>();
        }

        public AppState BuildAppState()
        {
            if (Presenter != null)
            {
                return new AppState()
                {
                    PresentationState = Presenter.GetPresentationState()
                };
            }
            else
            {
                return new AppState();
            }
        }
    }
}
