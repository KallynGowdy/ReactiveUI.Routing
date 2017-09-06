using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UWP
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private readonly ContentPresenter host;
        private readonly IViewLocator locator;

        public PagePresenter(ContentPresenter host, IViewLocator locator = null)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.locator = locator ?? Locator.Current.GetService<IViewLocator>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(observer =>
            {
                try
                {
                    var view = locator.ResolveView(request.ViewModel);

                    var disposable = view.WhenActivated(d =>
                    {
                        observer.OnNext(new PresenterResponse(view));
                        observer.OnCompleted();
                    });

                    view.ViewModel = request.ViewModel;
                    host.Content = view;

                    host.UpdateLayout();

                    return disposable;
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    throw;
                }
            });
        }

        public static IDisposable RegisterHost(ContentPresenter control)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.Register(new PagePresenter(control));
        }
    }
}
