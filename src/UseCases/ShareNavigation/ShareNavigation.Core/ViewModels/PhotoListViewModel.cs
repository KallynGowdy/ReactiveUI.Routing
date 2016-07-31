using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Routing;
using ShareNavigation.Services;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class PhotoListViewModel : RoutedViewModel<Unit, PhotoListViewModel.State>
    {
        private readonly ObservableAsPropertyHelper<Photo[]> loadedPhotos;
        private readonly ObservableAsPropertyHelper<byte[][]> loadedPhotoBytes;

        public class State
        {
            public Photo[] LoadedPhotos { get; set; }
        }

        public IPhotosService Service { get; }
        public ReactiveCommand<Photo[]> LoadPhotos { get; }
        public ReactiveCommand<Unit> Share { get; }
        public Photo[] LoadedPhotos => loadedPhotos.Value;
        public byte[][] LoadedPhotoData => loadedPhotoBytes.Value;

        public PhotoListViewModel(IRouter router = null, IPhotosService service = null)
            : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            LoadPhotos = ReactiveCommand.CreateAsyncTask(async o => await Service.GetPhotosAsync());
            Share = ReactiveCommand.CreateAsyncTask(async o => await ShareImpl());
            loadedPhotos = Resumed.Select(state => state?.LoadedPhotos).Merge(LoadPhotos)
                .ToProperty(this, vm => vm.LoadedPhotos, new Photo[0]);
            loadedPhotoBytes = this.WhenAnyValue(vm => vm.LoadedPhotos)
                .Where(photos => photos != null)
                .SelectMany(photos => Task.WhenAll(photos.Select(Service.GetPhotoDataAsync)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.LoadedPhotoData);
        }

        private Task ShareImpl()
        {
            return Router.ShowAsync<ShareViewModel>();
        }

        protected override State SuspendCoreSync()
        {
            return new State
            {
                LoadedPhotos = LoadedPhotos
            };
        }
    }
}
