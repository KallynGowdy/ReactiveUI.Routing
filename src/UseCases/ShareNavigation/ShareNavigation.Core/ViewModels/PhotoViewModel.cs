using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Routing;
using ShareNavigation.Services;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class PhotoViewModel : RoutedViewModel<PhotoViewModel.Params, PhotoViewModel.State>
    {
        public IPhotosService Service { get; set; }
        private readonly ObservableAsPropertyHelper<Photo> photo;
        private readonly ObservableAsPropertyHelper<byte[]> photoData;

        public class State
        {
        }

        public class Params
        {
            public Photo Photo { get; set; }
        }

        public Photo Photo => photo.Value;
        public byte[] PhotoData => photoData.Value;

        public PhotoViewModel(IRouter router = null, IPhotosService service = null) : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            photo = OnActivated.Select(p => p.Photo)
                .ToProperty(this, vm => vm.Photo);
            photoData = this.WhenAnyValue(vm => vm.Photo)
                .Where(p => p != null)
                .SelectMany(Service.GetPhotoDataAsync)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.PhotoData);
        }
    }
}