using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Routing;

namespace ShareNavigation.ViewModels
{
    public class PhotoViewModel : RoutedViewModel<PhotoViewModel.Params, PhotoViewModel.State>
    {
        private ObservableAsPropertyHelper<string> photoUrl;

        public class State
        {
        }

        public class Params
        {
            public Photo Photo { get; set; }
        }

        public string PhotoUrl => photoUrl.Value;

        public PhotoViewModel(IRouter router = null) : base(router)
        {
            photoUrl = Activated.Select(p => p.Photo.PhotoUrl).ToProperty(this, vm => vm.PhotoUrl);
        }
    }
}