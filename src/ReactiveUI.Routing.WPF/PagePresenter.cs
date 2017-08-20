using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.Presenters
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
                frame.Content = view;
                view.ViewModel = request.ViewModel;
                observer.OnNext(new PresenterResponse());
                observer.OnCompleted();

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
            var presenter = new PagePresenter(host);
            return resolver.Register<PagePresenterRequest>(request => request.ViewModel is TViewModel ? presenter : null);
        }
    }
}
