using System;
using System.Collections.Generic;
using System.Linq;
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

        public class State
        {
            public Photo[] LoadedPhotos { get; set; }
        }

        public IPhotosService Service { get; }
        public ReactiveCommand<Photo[]> LoadPhotos { get; }
        public ReactiveCommand<Unit> Share { get; }
        public Photo[] LoadedPhotos => loadedPhotos.Value;

        public PhotoListViewModel(IRouter router = null, IPhotosService service = null)
            : base(router)
        {
            Service = service ?? Locator.Current.GetService<IPhotosService>();
            LoadPhotos = ReactiveCommand.CreateAsyncTask(async o => await Service.GetPhotosAsync());
            Share = ReactiveCommand.CreateAsyncTask(async o => await Router.ShowAsync<ShareViewModel>());
            loadedPhotos = Observable.Merge(Resumed.Select(state => state?.LoadedPhotos), LoadPhotos)
                .ToProperty(this, vm => vm.LoadedPhotos);
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
