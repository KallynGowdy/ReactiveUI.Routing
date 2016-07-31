using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Xunit;

namespace ShareNavigation.Tests.ViewModels
{
    public class PhotoViewModelTests : RoutedViewModelTests<PhotoViewModel.Params, PhotoViewModel.State>
    {
        public PhotoViewModel ViewModel => (PhotoViewModel) RoutedViewModel;
        public IPhotosService Service { get; set; }

        public PhotoViewModelTests()
        {
            Service = Substitute.For<IPhotosService>();
            RoutedViewModel = new PhotoViewModel(Router, Service);
        }

        [Fact]
        public async Task Test_Loads_PhotoUrl_From_Params()
        {
            await ViewModel.InitAsync(new PhotoViewModel.Params()
            {
                Photo = new Photo
                {
                    PhotoUrl = "Test URL"
                }
            });

            ViewModel.Photo.PhotoUrl.Should().Be("Test URL");
        }
    }
}
