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

namespace ReactiveUI.Routing.Android
{
    public class PagePresenter : Presenter<PagePresenterRequest>
    {
        private static readonly Dictionary<Type, Type> viewModelToViewTypeMap = new Dictionary<Type, Type>();
        private readonly Application application;
        private readonly ActivityLifecycleCallbackHandler activityLifecycleCallbackHandler = new ActivityLifecycleCallbackHandler();

        public PagePresenter(Application application, ActivityLifecycleCallbackHandler handler = null)
        {
            this.application = application;
            this.activityLifecycleCallbackHandler =
                handler ?? Locator.Current.GetService<ActivityLifecycleCallbackHandler>();
        }

        protected override IObservable<PresenterResponse> PresentCore(PagePresenterRequest request)
        {
            return Observable.Create<PresenterResponse>(observer =>
            {
                var vmType = request.ViewModel.GetType();

                var viewType = viewModelToViewTypeMap[vmType];

                var disposable = activityLifecycleCallbackHandler.ActivityStarted
                    .FirstAsync(a => a.GetType() == viewType)
                    .Select(a => (IViewFor)a)
                    .Do(v => v.ViewModel = request.ViewModel)
                    .Select(v => new PresenterResponse(v))
                    .Subscribe(observer);

                Intent i = new Intent(application, viewType);

                // TODO: Find good solution for disabling the back stack
                // This might mean just overriding the back button
                i.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                application.StartActivity(i);

                return disposable;
            });
        }

        public static IDisposable Register(Type viewModel, Type view)
        {
            viewModelToViewTypeMap.Add(viewModel, view);
            return Disposable.Create(() => viewModelToViewTypeMap.Remove(viewModel));
        }

        //public static IDisposable RegisterHost(ContentControl host)
        //{
        //    var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
        //    return resolver.Register(new PagePresenter(host));
        //}

        //public static IDisposable RegisterHostFor<TViewModel>(ContentControl host)
        //{
        //    var resolver = Locator.Current.GetService<IMutablePresenterResolver>();
        //    return resolver.RegisterFor<PagePresenterRequest, TViewModel>(new PagePresenter(host));
        //}
    }
}