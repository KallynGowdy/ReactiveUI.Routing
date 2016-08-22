using System;
using System.Drawing;
using System.Reactive.Linq;
using CoreFoundation;
using CoreGraphics;
using UIKit;
using Foundation;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.iOS.Views
{
    [Register(nameof(PhotoViewController))]
    public class PhotoViewController : UIViewController, IViewFor<PhotoViewModel>
    {
        private UIImageView Photo { get; set; }

        public PhotoViewController()
        {
            this.WhenActivated(d =>
            {
                d(this.ViewModel.WhenAnyValue(vm => vm.PhotoData)
                    .Where(data => data != null)
                    .Select(data => UIImage.LoadFromData(NSData.FromArray(data)))
                    .BindTo(this, view => view.Photo.Image));
                ViewModel.LoadPhotoData.Execute(null);
            });
        }

        public override void ViewDidLoad()
        {
            BuildPhotoView();
            Title = "Photo!";
            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        private void BuildPhotoView()
        {
            var navFrame = NavigationController.NavigationBar.Frame;
            Photo = new UIImageView(new CGRect(navFrame.X, navFrame.Y + navFrame.Height, View.Frame.Width, View.Frame.Height))
            {
                ContentMode = UIViewContentMode.ScaleAspectFill
            };
            View.Add(Photo);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PhotoViewModel)value; }
        }

        public PhotoViewModel ViewModel { get; set; }
    }
}