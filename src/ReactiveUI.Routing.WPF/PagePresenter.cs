using System;
using System.Reactive.Linq;
using System.Windows.Controls;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.WPF
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private readonly ContentControl frame;
        private readonly IViewLocator viewLocator;

        public PagePresenter(ContentControl frame)
        {
            this.frame = frame;
            viewLocator = Locator.Current.GetService<IViewLocator>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(observer =>
            {
                var view = viewLocator.ResolveView(request.ViewModel);
                view.WhenActivated(d =>
                {
                    observer.OnNext(new PresenterResponse(view));
                    observer.OnCompleted();
                });
                frame.Content = view;
                view.ViewModel = request.ViewModel;

                return (() => { });
            });
        }

        public static IDisposable RegisterHost(ContentControl host)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.Register(new PagePresenter(host));
        }

        public static IDisposable RegisterHostFor<TViewModel>(ContentControl host)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.RegisterFor<PagePresenterRequest, TViewModel>(new PagePresenter(host));
        }
    }
}
