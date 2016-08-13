using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing;
using ShareNavigation.Core.ViewModels;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Splat;
using Xbehave;
using Xunit;

namespace ShareNavigation.Tests
{
    public class RouterTests : LocatorTest
    {
        private IRoutedAppConfig config;

        public RouterTests()
        {
            "Given a routed app config"
                .x(() =>
                        config =
                            new CompositeRoutedAppConfig(
                                new DefaultDependencies(),
                                new ShareNavigationDependencies(),
                                new TestRoutedAppConfig()));
        }

        [Scenario]
        public void Showing_PhotoListViewModel(
            IRouter router,
            IPagePresenter presenter,
            IRoutedAppHost appHost,
            IBlobCache cache)
        {
            using (Resolver.WithResolver())
            {
                "And a IBlobCache"
                    .x(() => cache = Substitute.For<IBlobCache>());
                "That is registered"
                    .x(() => Resolver.RegisterConstant(cache, typeof(IBlobCache)));
                "And a presenter for the view model"
                    .x(() => presenter = Substitute.For<IPagePresenter>());
                "That is registered"
                    .x(() => Resolver.RegisterConstant(presenter, typeof(IPagePresenter)));
                "And a IRoutedAppConfig"
                    .x(() => appHost = new RoutedAppHost(config));
                "When the app is started"
                    .x(async () => await appHost.StartAsync());
                "Then the navigator should have recieved a push call"
                    .x(() =>
                    {
                        Assert.Collection(Resolver.GetService<INavigator>().TransitionStack,
                            t => t.ViewModel.Should().BeOfType<PhotoListViewModel>());
                    });
                "And the presenter should have recieved a present call"
                    .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
            }
        }

        [Scenario]
        public void Showing_ShareViewModel(
            IRouter router,
            IPagePresenter presenter,
            IRoutedAppHost host)
        {
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPagePresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPagePresenter)));
            "And a IRoutedAppHost"
                .x(() => host = new RoutedAppHost(config));
            "That is started"
                .x(async () => await host.StartAsync());
            "And a IRouter"
                .x(() => router = Resolver.GetService<IRouter>());
            "When I show the ShareViewModel"
                .x(async () => await router.ShowAsync<ShareViewModel>());
            "Then the navigator should have recieved a push call"
                .x(() =>
                {
                    Assert.Collection(Resolver.GetService<INavigator>().TransitionStack,
                        t => t.ViewModel.Should().BeOfType<PhotoListViewModel>(),
                        t => t.ViewModel.Should().BeOfType<ShareViewModel>());
                });
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }

        [Scenario]
        public void Showing_PhotoViewModel_From_ShareViewModel(
            IRouter router,
            PhotoListViewModel photoListViewModel,
            ShareViewModel shareViewModel,
            PhotoViewModel.Params parameters,
            IPagePresenter presenter,
            IRoutedAppHost host)
        {
            "And the PhotoViewModel Parameters"
                .x(() => parameters = new PhotoViewModel.Params()
                {
                    Photo = new Photo()
                    {
                        PhotoUrl = "Url"
                    }
                });
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPagePresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPagePresenter)));
            "And a IRoutedAppHost"
                .x(() => host = new RoutedAppHost(config));
            "That is started"
                .x(async () => await host.StartAsync());
            "And a IRouter"
                .x(() => router = Resolver.GetService<IRouter>());
            "And I'm at ShareViewModel"
                .x(async () => await router.ShowAsync<ShareViewModel>());
            "When I Show the PhotoViewModel"
                .x(async () => await router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(parameters));
            "Then I Should have navigated to the PhotoViewModel"
                .x(() =>
                {
                    Assert.Collection(Resolver.GetService<INavigator>().TransitionStack,
                        t => t.ViewModel.Should().BeOfType<PhotoListViewModel>(),
                        t => t.ViewModel.Should().BeOfType<PhotoViewModel>());
                });
            "And the PhotoViewModel presenter should have recieved a present call"
                .x(() => presenter.Received().PresentAsync(Arg.Any<object>(), Arg.Any<object>()));
        }
    }
}
