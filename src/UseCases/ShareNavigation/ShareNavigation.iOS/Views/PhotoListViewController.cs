using System.Reactive.Linq;
using Akavache;
using ReactiveUI;
using ShareNavigation.ViewModels;
using UIKit;

namespace ShareNavigation.Views
{
    public partial class PhotoListViewController : ReactiveViewController, IViewFor<PhotoListViewModel>
    {
        static bool UserInterfaceIdiomIsPhone => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

        public PhotoListViewController()
            : base (UserInterfaceIdiomIsPhone ? "TestViewController_iPhone" : "TestViewController_iPad", null)
        {
            this.WhenActivated(d =>
            {
                // Bindings
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PhotoListViewModel) value; }
        }

        public PhotoListViewModel ViewModel { get; set; }
    }
}

