using System;

namespace ReactiveUI.Routing.Presentation
{
    public interface IPresenter
    {
        IObservable<PresenterResponse> Present(PresenterRequest request);
    }
}
