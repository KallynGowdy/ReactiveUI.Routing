using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Routing;
using ShareNavigation.Core.ViewModels;
using ShareNavigation.Services;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class PhotoViewModel : RoutedViewModel<PhotoViewModel.Params, PhotoViewModel.State>
    {
        public IPhotosService Service { get; }
        private readonly ObservableAsPropertyHelper<Photo> photo;
        private readonly ObservableAsPropertyHelper<byte[]> photoData;
        private readonly ObservableAsPropertyHelper<bool> isLoading;

        public class State
        {
        }

        public class Params
        {
            public Photo Photo { get; set; }
        }

        public Photo Photo => photo.Value;
        public byte[] PhotoData => photoData.Value;
        public bool IsLoading => isLoading.Value;

        public ReactiveCommand<Unit, byte[]> LoadPhotoData { get; }

        public PhotoViewModel(IRouter router = null, IPhotosService service = null) : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            photo = OnActivated.Select(p => p.Photo)
                .ToProperty(this, vm => vm.Photo);
            var canLoadData = this.WhenAnyValue(vm => vm.Photo)
                .Select(p => p != null);
            LoadPhotoData = ReactiveCommand.CreateFromTask(async o => await LoadPhotoDataImpl(), canLoadData);
            photoData = LoadPhotoData
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.PhotoData);
            isLoading = LoadPhotoData.IsExecuting
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.IsLoading);
            this.WhenAnyValue(vm => vm.Photo)
                .Select(x => Unit.Default)
                .InvokeCommand(this, vm => vm.LoadPhotoData);
        }

        private Task<byte[]> LoadPhotoDataImpl()
        {
            return Service.GetPhotoDataAsync(Photo);
        }
    }
}