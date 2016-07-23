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
        public async Task Showing_PhotoListViewModel(
            IRouter router,
            INavigator navigator,
            IPhotoListPresenter presenter)
        {
            "Given a INavigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "And a IRouter"
                .x(async () => router = await new RouterConfig().BuildRouterAsync(navigator));
            "And a PhotoListViewModel registration"
                .x(() => Resolver.Register(() => new PhotoListViewModel(), typeof(PhotoListViewModel)));
            "And a presenter for the view model"
                .x(() => presenter = Substitute.For<IPhotoListPresenter>());
            "That is registered"
                .x(() => Resolver.Register(() => presenter, typeof(IPhotoListPresenter)));
            "When I show PhotoListViewModel"
                .x(async () => await router.ShowAsync<PhotoListViewModel>());
            "Then the navigator should have recieved a push call"
                .x(() =>
                {
                    navigator.Received(1).PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoListViewModel));
                });
            "And the presenter should have recieved a present call"
                .x(() => presenter.Received(1).PresentAsync(Arg.Any<object>(), Unit.Default));
        }

        [Scenario]
        public async Task Showing_PhotoViewModel_From_ShareViewModel(
            IRouter router, 
            INavigator navigator, 
            PhotoListViewModel photoListViewModel, 
            ShareViewModel shareViewModel, 
            IPhotosService photosService, 
            PhotoViewModel.Params parameters)
        {
            "Given a navigator"
                .x(() => navigator = Substitute.For<INavigator>());
            "Given a IRouter"
                .x(async () => router = await new RouterConfig().BuildRouterAsync(navigator));
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
                }));
            "And the PhotoViewModel Parameters"
                .x(() => parameters = new PhotoViewModel.Params());
            "When I Show the PhotoViewModel"
                .x(async () => await router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(parameters));
            "Then I Should have navigated to the PhotoViewModel"
                .x(() =>
                {
                    navigator.Received().PushAsync(Arg.Is<Transition>(t => t.ViewModel is PhotoViewModel));
                });
        }
    }
}
