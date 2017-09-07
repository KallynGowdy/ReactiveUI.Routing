using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UWP
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private readonly Frame host;
        private readonly IViewLocator locator;

        public PagePresenter(Frame host, IViewLocator locator = null)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.locator = locator ?? Locator.Current.GetService<IViewLocator>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(o =>
            {
                try
                {
                    var view = locator.ResolveView(request.ViewModel);
                    view.ViewModel = request.ViewModel;
                    host.Navigate(view.GetType());

                    o.OnNext(new PresenterResponse(view));
                    o.OnCompleted();

                }
                catch (Exception ex)
                {
                    o.OnError(ex);
                }
                return new CompositeDisposable();
            });
        }

        public static IDisposable RegisterHost(Frame control)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.Register(new PagePresenter(control));
        }
    }
}
