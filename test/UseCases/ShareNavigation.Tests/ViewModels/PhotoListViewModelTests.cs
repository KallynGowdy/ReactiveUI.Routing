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
using ShareNavigation.Core.ViewModels;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Xunit;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace ShareNavigation.Tests.ViewModels
{
    public class PhotoListViewModelTests : LocatorTest
    {
        public PhotoListViewModel ViewModel { get; set; }
        public IPhotosService Service { get; }
        public IRouter Router { get; set; }

        public PhotoListViewModelTests()
        {
            Service = Substitute.For<IPhotosService>();
            Router = Substitute.For<IRouter>();
            ViewModel = new PhotoListViewModel(Router, Service);
        }

        [Fact]
        public void Test_Retrieves_Service_From_Locator()
        {
            Resolver.Register(() => Service, typeof(IPhotosService));
            var viewModel = new PhotoListViewModel(Router);
            viewModel.Service.Should().Be(Service);
        }

        [Fact]
        public async Task Test_Share_Navigates_To_ShareViewModel()
        {
            await ViewModel.Share.Execute();

            Router.Received(1).DispatchAsync(Arg.Is<ShowViewModelAction>(a =>
                a.ActivationParams.Type == typeof(ShareViewModel) &&
                Equals(a.ActivationParams.Params, Unit.Default)));
        }

    }
}
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
