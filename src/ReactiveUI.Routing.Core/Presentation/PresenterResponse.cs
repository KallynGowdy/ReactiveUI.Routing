using System;

namespace ReactiveUI.Routing.Presentation
{
    public class PresenterResponse
    {
        public IViewFor PresentedView { get; }

        public PresenterResponse(IViewFor presentedView)
        {
            this.PresentedView = presentedView ?? throw new ArgumentNullException(nameof(presentedView));
        }
    }
}