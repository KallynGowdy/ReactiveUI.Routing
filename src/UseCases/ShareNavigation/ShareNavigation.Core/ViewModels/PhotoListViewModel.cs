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
using ShareNavigation.Core.ViewModels;
using ShareNavigation.Services;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class PhotoListViewModel : RoutedViewModel<Unit, PhotoListViewModel.State>
    {
        private readonly ObservableAsPropertyHelper<Photo[]> loadedPhotos;
        private readonly ObservableAsPropertyHelper<byte[][]> loadedPhotoBytes;
        private readonly ObservableAsPropertyHelper<bool> isLoading;

        public class State
        {
            public Photo[] LoadedPhotos { get; set; }
        }

        public IPhotosService Service { get; }
        public ReactiveCommand<Unit, Photo[]> LoadPhotos { get; }
        public ReactiveCommand<Unit, byte[][]> LoadPhotoData { get; }
        public ReactiveCommand<Unit, Unit> Share { get; }
        public ReactiveCommand<int, Unit> ShowPhoto { get; }
        public Photo[] LoadedPhotos => loadedPhotos.Value;
        public byte[][] LoadedPhotoData => loadedPhotoBytes.Value;
        public bool IsLoading => isLoading.Value;

        public PhotoListViewModel(IRouter router = null, IPhotosService service = null)
            : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            var canLoad = Resumed.FirstAsync().Select(r => r?.LoadedPhotos == null);
            LoadPhotos = ReactiveCommand.CreateFromTask(async o => await Service.GetPhotosAsync(), canLoad);
            var canLoadData = this.WhenAnyValue(vm => vm.LoadedPhotos)
                .Select(p => p != null);
            loadedPhotos = Resumed.Select(state => state?.LoadedPhotos).Merge(LoadPhotos)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.LoadedPhotos, new Photo[0]);
            LoadPhotoData = ReactiveCommand.CreateFromTask(async o => await LoadPhotoDataImpl(), canLoadData);
            Share = ReactiveCommand.CreateFromTask(async o => await ShareImpl());
            ShowPhoto = ReactiveCommand.CreateFromTask<int>(async p => await ShowPhotoImpl(p));
            loadedPhotoBytes = LoadPhotoData
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.LoadedPhotoData);
            isLoading = LoadPhotos.IsExecuting.CombineLatest(LoadPhotoData.IsExecuting, (l, ld) => l || ld)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.IsLoading);

            this.WhenAnyValue(vm => vm.LoadedPhotos)
                .Select(x => Unit.Default)
                .InvokeCommand(this, vm => vm.LoadPhotoData);
        }

        private async Task ShowPhotoImpl(int index)
        {
            var photo = LoadedPhotos[index];
            await Router.ShowAsync<PhotoViewModel, PhotoViewModel.Params>(new PhotoViewModel.Params()
            {
                Photo = photo
            });
        }

        private async Task<byte[][]> LoadPhotoDataImpl()
        {
            return await Task.WhenAll(LoadedPhotos.Select(Service.GetPhotoDataAsync));
        }

        private Task ShareImpl()
        {
            return Router.ShowAsync<ShareViewModel>();
        }

        protected override State GetStateCoreSync()
        {
            return new State
            {
                LoadedPhotos = LoadedPhotos
            };
        }
    }
}
