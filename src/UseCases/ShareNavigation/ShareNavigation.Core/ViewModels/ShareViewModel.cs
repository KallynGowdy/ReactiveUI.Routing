using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Routing;
using ShareNavigation.Services;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class ShareViewModel : RoutedViewModel<Unit, ShareViewModel.State>
    {
        public IPhotosService Service { get; set; }
        private string photoUrl;

        public class State
        {
        }

        public string PhotoUrl
        {
            get { return photoUrl; }
            set { this.RaiseAndSetIfChanged(ref photoUrl, value); }
        }

        public ReactiveCommand<Unit> Share { get; }

        public ShareViewModel(IRouter router = null, IPhotosService service = null)
            : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            Share = ReactiveCommand.CreateAsyncTask(async o =>
            {
                var photo = new Photo
                {
                    PhotoUrl = PhotoUrl
                };
                await Service.SharePhotoAsync(photo);
                await Router.ShowAsync(typeof(PhotoViewModel), new PhotoViewModel.Params
                {
                    Photo = photo
                });
            });
        }
    }
}
