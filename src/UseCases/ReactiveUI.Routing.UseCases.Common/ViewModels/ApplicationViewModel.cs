using System;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    public class ApplicationViewModel : ReactiveObject
    {
        public IAppPresenter Presenter { get; set; }
        public IReactiveRouter Router { get; set; }

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
            //Locator.CurrentMutable.InitializeRouting();
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
