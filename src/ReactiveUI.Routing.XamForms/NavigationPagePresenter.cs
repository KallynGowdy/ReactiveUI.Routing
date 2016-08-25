using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Splat;
using Xamarin.Forms;

namespace ReactiveUI.Routing.XamForms
{
    /// <summary>
    /// Defines a <see cref="IPagePresenter"/> that presents <see cref="Page"/> views in a <see cref="Xamarin.Forms.NavigationPage"/>.
    /// </summary>
    public class NavigationPagePresenter : BasePresenter, IPagePresenter
    {
        private readonly Application application;
        public NavigationPage NavigationPage { get; private set; }
        private readonly List<Page> pages = new List<Page>(2);

        public NavigationPagePresenter(Application application = null)
        {
            this.application = application ?? Locator.Current.GetService<Application>();
        }

        public override int GetAffinityForView(Type view)
        {
            return typeof(Page).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo()) ? 1000 : 0;
        }

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            var viewModelType = viewModel.GetType();
            var viewType = ResolveViewTypeForViewModelType(viewModelType);
            var view = CreateViewFromType(viewType);
            view.ViewModel = viewModel;
            var page = (Page)view;
            await PushPageAsync(page);
            return new ScheduledDisposable(RxApp.MainThreadScheduler, new ActionDisposable(() => PopPage(page)));
        }

        private async Task PushPageAsync(Page page)
        {
            pages.Add(page);
            if (NavigationPage == null)
            {
                NavigationPage = new NavigationPage();
                await PushPageCoreAsync(page);
                await Observable.Start(() =>
                {
                    application.MainPage = NavigationPage;
                }, RxApp.MainThreadScheduler);
            }
            else
            {
                await PushPageCoreAsync(page);
            }
        }

        private async Task PushPageCoreAsync(Page page)
        {
            await NavigationPage.Navigation.PushAsync(page, true);
            NotifyViews();
        }

        private void PopPage(Page page)
        {
            if (!pages.Remove(page)) return;
            NavigationPage.Navigation.RemovePage(page);
            NotifyViews();
        }

        private void NotifyViews()
        {
            foreach (var p in pages.Take(pages.Count - 1))
            {
                NotifyViewDeActivated((ReactiveUI.IActivatable)p);
            }
            NotifyViewActivated((ReactiveUI.IActivatable)pages.Last());
        }
    }
}
