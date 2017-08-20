using System;

namespace ReactiveUI.Routing.Presentation
{
    public interface IAppPresenter : IPresenter
    {
        IObservable<PresenterResponse> PresentPage(object viewModel);
    }
}
