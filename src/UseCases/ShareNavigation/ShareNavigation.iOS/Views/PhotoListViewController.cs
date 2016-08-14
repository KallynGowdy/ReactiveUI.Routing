using System;
using System.Reactive.Linq;
using Akavache;
using ReactiveUI;
using ShareNavigation.ViewModels;
using UIKit;

namespace ShareNavigation.Views
{
    public partial class PhotoListViewController : UIViewController //, IViewFor<PhotoListViewModel>
    {
        //static bool UserInterfaceIdiomIsPhone => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

        public PhotoListViewController()
            : base("TestUI", null)
        {
        }

        //object IViewFor.ViewModel
        //{
        //    get { return ViewModel; }
        //    set { ViewModel = (PhotoListViewModel) value; }
        //}

        //public PhotoListViewModel ViewModel { get; set; }
    }
}

