using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveUI.Routing.Core.Presentation
{
    public interface IPresenter
    {
        IObservable<PresenterResponse> Present(object request);
    }
}
