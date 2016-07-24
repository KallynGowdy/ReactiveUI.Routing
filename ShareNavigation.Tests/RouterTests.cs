using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing;
using ShareNavigation.Presenters;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Splat;
using Xbehave;
using Xunit;

namespace ShareNavigation.Tests
{
    public class RouterTests : LocatorTest
    {
        public RouterTests()
        {
        }

        [Scenario]
        public void Showing_PhotoListViewModel(
            IRouter router,
            INavigator navigator,
            IPhotoListPresenter presenter)
        {
            "Given a INavigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a IRouter"
                .x(async () => router = await new RoutedAppConfig().BuildRouterAsync());
            "And a PhotoListViewModel registration"
                .x(() => Resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPhotoListPresenter>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(presenter, typeof(IPhotoListPresenter)));
            "When I show PhotoListViewModel"
                .x(async () => await router.ShowAsync<PhotoListViewModel>());
            "Then the navigator should have recieved a push call"
                .x(() =>
                {
                    navigator.Received(1).PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoListViewModel));
                });
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_ShareViewModel(
            IRouter router,
            INavigator navigator,
            ISharePresenter presenter)
        {
            "Given a navigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a IRouter"
                .x(async () => router = await new RoutedAppConfig().BuildRouterAsync());
            "And a ShareViewModel registration"
                .x(() => Resolver.Register(() => new ShareViewModel(router), typeof(ShareViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<ISharePresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(ISharePresenter)));
            "When I show the ShareViewModel"
                .x(async () => await router.ShowAsync<ShareViewModel>());
            "Then the navigator should have recieved a push call"
                .x(() => navigator.Received(1).PushAsync(Arg.Is<Transition>(t => t.ViewModel is ShareViewModel)));
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_PhotoViewModel_From_ShareViewModel(
            IRouter router,
            INavigator navigator,
            PhotoListViewModel photoListViewModel,
            ShareViewModel shareViewModel,
            IPhotosService photosService,
            PhotoViewModel.Params parameters,
            IPhotoPresenter presenter)
        {
            "Given a navigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a IRouter"
                .x(async () => router = await new RoutedAppConfig().BuildRouterAsync());
            "And a IPhotosService"
                .x(() => photosService = Substitute.For<IPhotosService>());
            "And a PhotoListViewModel"
                .x(() => photoListViewModel = new PhotoListViewModel(router, photosService));
            "And a ShareViewModel"
                .x(() => shareViewModel = new ShareViewModel(router, photosService));
            "And I'm at ShareViewModel"
                .x(() => navigator.Peek().Returns(new Transition()
                {
                    ViewModel = shareViewModel
                }, new Transition()
                {
                    ViewModel = photoListViewModel
                }));
            "And the PhotoViewModel Parameters"
                .x(() => parameters = new PhotoViewModel.Params()
                {
                    Photo = new Photo()
                    {
                        PhotoUrl = "Url"
                    }
                });
            "And a PhotoViewModel registration"
                .x(() => Resolver.Register(() => new PhotoViewModel(), typeof(PhotoViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPhotoPresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPhotoPresenter)));
            "When I Show the PhotoViewModel"
                .x(async () => await router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(parameters));
            "Then I Should have navigated to the PhotoViewModel"
                .x(() =>
                {
                    navigator.Received(1).PopAsync();
                    navigator.Received().PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoViewModel));
                });
            "And the PhotoViewModel presenter should have recieved a present call"
                .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }
    }
}
