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
        private readonly ContentControl host;
        private readonly IViewLocator locator;

        public PagePresenter(ContentControl host, IViewLocator locator = null)
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

                    var disposable = view.WhenActivated(d =>
                    {
                        o.OnNext(new PresenterResponse(view));
                        o.OnCompleted();
                    });

                    view.ViewModel = request.ViewModel;
                    host.Content = view;

                    return disposable;
                }
                catch (Exception ex)
                {
                    o.OnError(ex);
                    throw;
                }
            });
        }

        public static IDisposable RegisterHost(ContentControl control)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.Register(new PagePresenter(control));
        }
    }
}
