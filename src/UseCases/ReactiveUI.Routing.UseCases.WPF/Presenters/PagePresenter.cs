using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReactiveUI.Routing.Core.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.Presenters
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private readonly Frame frame;
        private readonly IViewLocator viewLocator;

        public PagePresenter(Frame frame)
        {
            this.frame = frame;
            viewLocator = Locator.Current.GetService<IViewLocator>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(observer =>
            {
                var view = viewLocator.ResolveView(request.ViewModel);
                frame.Navigate(view);
                view.ViewModel = request.ViewModel;
                observer.OnNext(new PresenterResponse());
                observer.OnCompleted();

                return (() => { });
            });
        }
    }
}
