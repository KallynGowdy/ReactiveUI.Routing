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
        private readonly IRouter router;
        private readonly Application application;
        public NavigationPage NavigationPage { get; private set; }
        private readonly List<Page> pages = new List<Page>(2);

        public NavigationPagePresenter(IRouter router = null, Application application = null)
        {
            this.router = router ?? Locator.Current.GetService<IRouter>();
            this.application = application ?? Locator.Current.GetService<Application>();
        }

        public override int GetAffinityForView(Type view)
        {
            return typeof(Page).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo()) ? 1000 : 0;
        }

        public override async Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            if (pages.Any(p => ((IViewFor) p).ViewModel == viewModel)) return null;
            var viewModelType = viewModel.GetType();
            var viewType = ResolveViewTypeForViewModelType(viewModelType);
            var view = CreateViewFromType(viewType);
            view.ViewModel = viewModel;
            var page = (Page) view;
            await PushPageAsync(page);
            return new ScheduledDisposable(RxApp.MainThreadScheduler, new ActionDisposable(() => PopPage(page)));
        }

        private async Task PushPageAsync(Page page)
        {
            pages.Add(page);
            if (NavigationPage == null)
            {
                if (application.MainPage == null)
                {
                    NavigationPage = new NavigationPage();
                    SetupNavigationPage();
                    await PushPageCoreAsync(page);
                    application.MainPage = NavigationPage;
                }
                else
                {
                    NavigationPage = (NavigationPage)application.MainPage;
                    SetupNavigationPage();
                    NotifyCurrentPage();
                }
            }
            else
            {
                await PushPageCoreAsync(page);
            }
        }

        private void SetupNavigationPage()
        {
            NavigationPage.Popped += NavigationPageOnPopped;
            NavigationPage.Disappearing += NavigationPageOnDisappearing;
            NavigationPage.Appearing += NavigationPageOnAppearing;
        }

        private void NavigationPageOnAppearing(object sender, EventArgs eventArgs)
        {
            NotifyCurrentPage();
        }

        private void NavigationPageOnDisappearing(object sender, EventArgs eventArgs)
        {
            NotifyPages(pages);
        }

        private void NavigationPageOnPopped(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (!pages.Remove(navigationEventArgs.Page)) return;
            router.BackAsync();
            NotifyViews();
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
            NotifyPages(pages.Take(pages.Count - 1));
            NotifyCurrentPage();
        }

        private void NotifyPages(IEnumerable<Page> pagesToNotify)
        {
            foreach (var p in pagesToNotify)
            {
                NotifyViewDeActivated((ReactiveUI.IActivatable)p);
            }
        }

        private void NotifyCurrentPage()
        {
            NotifyViewActivated((ReactiveUI.IActivatable)pages.Last());
        }
    }
}
