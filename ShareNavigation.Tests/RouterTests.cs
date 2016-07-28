using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
                .x(() => navigator = Resolver.GetService<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
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
                    Assert.Collection(navigator.TransitionStack,
                        t => t.ViewModel.Should().BeOfType<PhotoListViewModel>());
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
                .x(() => navigator = Resolver.GetService<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
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
                .x(() =>
                {
                    Assert.Collection(navigator.TransitionStack,
                        t => t.ViewModel.Should().BeOfType<PhotoListViewModel>(),
                        t => t.ViewModel.Should().BeOfType<ShareViewModel>());
                });
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_PhotoViewModel_From_ShareViewModel(IRouter router, INavigator navigator, PhotoListViewModel photoListViewModel, ShareViewModel shareViewModel, IPhotosService photosService, PhotoViewModel.Params parameters, IPresenter presenter, IActivator activator)
        {
            "Given a RoutedAppConfig"
                .x(() => new RoutedAppConfig().RegisterDependencies(Resolver));
            "And a navigator"
                .x(() => navigator = Resolver.GetService<INavigator>());
            "That is registered"
                .x(() => Resolver.RegisterConstant(navigator, typeof(INavigator)));
            "And a IPhotosService"
                .x(() => photosService = Substitute.For<IPhotosService>());
            "And the PhotoViewModel Parameters"
                .x(() => parameters = new PhotoViewModel.Params()
                {
                    Photo = new Photo()
                    {
                        PhotoUrl = "Url"
                    }
                });
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPresenter)));
            "And a IActivator"
                .x(() => activator = Resolver.GetService<IActivator>());
            "And a IRouter"
                .x(async () => router = await activator.ActivateAsync<IRouter, RouterParams>(Resolver.GetService<RouterParams>()));
            "And I'm at ShareViewModel"
                .x(async () =>
                {
                    await navigator.PushAsync(new Transition()
                    {
                        ViewModel = new ShareViewModel()
                    });
                });
            "When I Show the PhotoViewModel"
                .x(async () => await router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(parameters));
            "Then I Should have navigated to the PhotoViewModel"
                .x(() =>
                {
                    Assert.Collection(navigator.TransitionStack,
                        t => t.ViewModel.Should().BeOfType<PhotoListViewModel>(),
                        t => t.ViewModel.Should().BeOfType<PhotoViewModel>());
                });
            "And the PhotoViewModel presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }
    }
}
