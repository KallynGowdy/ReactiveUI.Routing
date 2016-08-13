using System.Reactive;
using ReactiveUI;
using ReactiveUI.Routing;
using ShareNavigation.Services;
using ShareNavigation.ViewModels;
using Splat;

namespace ShareNavigation.Core.ViewModels
{
    public class ShareViewModel : RoutedViewModel<Unit, ShareViewModel.State>
    {
        public IPhotosService Service { get; set; }
        private string photoUrl;

        public class State
        {
            public string PhotoUrl { get; set; }
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
            var canShare = this.WhenAny(vm => vm.PhotoUrl, url => !string.IsNullOrEmpty(url.Value));
            Share = ReactiveCommand.CreateAsyncTask(canShare, async o =>
            {
                var photo = new Photo
                {
                    PhotoUrl = PhotoUrl
                };
                await Service.SharePhotoAsync(photo);
                await Router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(new PhotoViewModel.Params
                {
                    Photo = photo
                });
                await Router.ShowAsync<ToastViewModel, ToastViewModel.Params>(new ToastViewModel.Params()
                {
                    Message = "Your photo was created!"
                });
            });
        }

        protected override void ResumeCoreSync(State storedState)
        {
            base.ResumeCoreSync(storedState);
            PhotoUrl = storedState.PhotoUrl;
        }

        protected override State SuspendCoreSync()
        {
            var state = base.SuspendCoreSync();
            state.PhotoUrl = PhotoUrl;
            return state;
        }
    }
}
