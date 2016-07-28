using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Actions;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Xunit;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace ShareNavigation.Tests.ViewModels
{
    public class ShareViewModelTests : RoutedViewModelTests<Unit, ShareViewModel.State>
    {
        public ShareViewModel ViewModel => (ShareViewModel)RoutedViewModel;
        public IPhotosService Service { get; }
        public ShareViewModelTests()
        {
            Service = Substitute.For<IPhotosService>();
            RoutedViewModel = new ShareViewModel(Router, Service);
        }

        [Fact]
        public async Task Test_Share_Shows_PhotoViewModel()
        {
            ViewModel.PhotoUrl = "URL";
            await ViewModel.Share.ExecuteAsync();
            Router.Received(1).DispatchAsync(Arg.Is<ShowViewModelAction>(
                a => a.ActivationParams.Type == typeof(PhotoViewModel) &&
                ((PhotoViewModel.Params)a.ActivationParams.Params).Photo.PhotoUrl == "URL"));
        }

        [Fact]
        public async Task Test_Share_Saves_Photo_To_Service()
        {
            ViewModel.PhotoUrl = "URL";
            await ViewModel.Share.ExecuteAsync();
            Service.Received(1).SharePhotoAsync(Arg.Is<Photo>(p => p.PhotoUrl == "URL"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Test_Cannot_Share_When_Url_Is_Invalid(string url)
        {
            ViewModel.PhotoUrl = url;
            ViewModel.Share.CanExecute(null).Should().BeFalse();
        }
    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
