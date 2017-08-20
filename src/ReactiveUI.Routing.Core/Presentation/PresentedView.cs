using System;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines a class that contains information about a view that has been presented.
    /// It contains the resolved view as well as the presenter request and presenter that 
    /// created it.
    /// </summary>
    public class PresentedView
    {
        public PresenterResponse Response { get; }
        public PresenterRequest Request { get; }
        public IPresenter Presenter { get; }

        public PresentedView(PresenterResponse response, PresenterRequest request, IPresenter presenter)
        {
            this.Response = response ?? throw new ArgumentNullException(nameof(response));
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }
    }
}