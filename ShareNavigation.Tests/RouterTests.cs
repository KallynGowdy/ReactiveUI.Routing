using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing;
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
        public void Showing_PhotoListViewModel(IRouter router, INavigator navigator, IPresenter presenter, IActivator activator)
        {
            "Given a routed app config"
                .x(() => new RoutedAppConfig().RegisterDependencies(Resolver));
            "And a INavigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a PhotoListViewModel registration"
                .x(() => Resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPresenter>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(presenter, typeof(IPresenter)));
            "And a IActivator"
                .x(() => activator = Resolver.GetService<IActivator>());
            "When the router is initialized"
                .x(async () => router = await activator.ActivateAsync<IRouter, RouterParams>(Resolver.GetService<RouterParams>()));
            "Then the navigator should have recieved a push call"
                .x(() =>
                {
                    navigator.Received().PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoListViewModel));
                });
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_ShareViewModel(
            IRouter router,
            INavigator navigator,
            IPresenter presenter,
            IActivator activator)
        {
            "Given a RoutedAppConfig"
                .x(() => new RoutedAppConfig().RegisterDependencies(Resolver));
            "And a navigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a PhotoListViewModel"
                .x(() =>
                {
                    Resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel));
                    Resolver.Register(() => Substitute.For<IPresenter>(), typeof(IPresenter));
                });
            "And a ShareViewModel"
                .x(() => Resolver.Register(() => new ShareViewModel(router), typeof(ShareViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPresenter)));
            "And a IActivator"
                .x(() => activator = Resolver.GetService<IActivator>());
            "And a IRouter"
                .x(async () => router = await activator.ActivateAsync<IRouter, RouterParams>(Resolver.GetService<RouterParams>()));
            "When I show the ShareViewModel"
                .x(async () => await router.ShowAsync<ShareViewModel>());
            "Then the navigator should have recieved a push call"
                .x(() => navigator.Received(1).PushAsync(Arg.Is<Transition>(t => t.ViewModel is ShareViewModel)));
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_PhotoViewModel_From_ShareViewModel(IRouter router, INavigator navigator, PhotoListViewModel photoListViewModel, ShareViewModel shareViewModel, IPhotosService photosService, PhotoViewModel.Params parameters, IPresenter presenter, IActivator activator)
        {
            "Given a RoutedAppConfig"
                .x(() => new RoutedAppConfig().RegisterDependencies(Resolver));
            "And a navigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a IPhotosService"
                .x(() => photosService = Substitute.For<IPhotosService>());
            "And a PhotoListViewModel"
                .x(() =>
                {
                    photoListViewModel = new PhotoListViewModel();
                    Resolver.Register(() => photoListViewModel, typeof(PhotoListViewModel));
                    Resolver.Register(() => Substitute.For<IPresenter>(), typeof(IPresenter));
                });
            "That is registered"
                .x(() => Resolver.RegisterConstant(photoListViewModel, typeof(PhotoListViewModel)));
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
                .x(() => presenter = Substitute.For<IPresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPresenter)));
            "And a IActivator"
                .x(() => activator = Resolver.GetService<IActivator>());
            "And a IRouter"
                .x(async () => router = await activator.ActivateAsync<IRouter, RouterParams>(Resolver.GetService<RouterParams>()));
            "When I Show the PhotoViewModel"
                .x(async () => await router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(parameters));
            "Then I Should have navigated to the PhotoViewModel"
                .x(() =>
                {
                    navigator.Received(1).PopAsync();
                    navigator.Received().PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoViewModel));
                });
            "And the PhotoViewModel presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }
    }
}
