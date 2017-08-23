using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI.Routing.Presentation;
using Splat;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ReactiveUI.Routing.Android
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private readonly FragmentManager host;
        private readonly int containerId;
        private readonly IViewLocator viewLocator;

        public PagePresenter(FragmentManager host, int containerId, IViewLocator viewLocator = null)
        {
            this.host = host;
            this.containerId = containerId;
            this.viewLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(observer =>
            {
                try
                {
                    var view = viewLocator.ResolveView(request.ViewModel);
                    var fragment = (Fragment)view;

                    var disposable = view.WhenActivated(d =>
                    {
                        observer.OnNext(new PresenterResponse(view));
                        observer.OnCompleted();
                    });

                    var transaction = host.BeginTransaction();
                    transaction.Replace(containerId, fragment);
                    transaction.Commit();

                    view.ViewModel = request.ViewModel;

                    return disposable;
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    throw;
                }
            });
        }

        public static IDisposable RegisterHost(FragmentManager host, int containerId)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.Register(new PagePresenter(host, containerId));
        }

        public static IDisposable RegisterHostFor<TViewModel>(FragmentManager host, int containerId)
        {
            var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
            return resolver.RegisterFor<PagePresenterRequest, TViewModel>(new PagePresenter(host, containerId));
        }
    }
}