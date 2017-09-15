using System;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    public class ApplicationViewModel : ReactiveObject
    {
        public IAppPresenter Presenter { get; set; }
        public IReactiveRouter Router { get; set; }

        private ReactiveAppState state;

        public ApplicationViewModel()
        {
        }

        public void LoadState(ReactiveAppState state)
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
            Presenter = Locator.Current.GetService<IAppPresenter>();
        }

        public ReactiveAppState BuildAppState()
        {
            if (Presenter != null)
            {
                return new ReactiveAppState()
                {
                    PresentationState = Presenter.GetPresentationState()
                };
            }
            else
            {
                return new ReactiveAppState();
            }
        }
    }
}
