using System;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using ShareNavigation.Core.ViewModels;
using UIKit;

namespace ShareNavigation.iOS.Views
{
    [Register(nameof(ShareViewController))]
    public class ShareViewController : UIViewController, IViewFor<ShareViewModel>
    {
        private UITextField PhotoUrl { get; set; }
        private UIButton Share { get; set; }

        public ShareViewController()
        {
            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.PhotoUrl, view => view.PhotoUrl.Text));
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.Share));
            });
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            Title = "Share!";
            BuildPhotoUrlField();
            BuildShareButton();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        private void BuildPhotoUrlField()
        {
            var navbarFrame = NavigationController.NavigationBar.Frame;
            PhotoUrl = new UITextField(new CGRect(10, navbarFrame.Y + navbarFrame.Height + 10, View.Frame.Width - 20, 40))
            {
                Placeholder = "Photo URL",
                BorderStyle = UITextBorderStyle.RoundedRect,
                ReturnKeyType = UIReturnKeyType.Done
            };
            PhotoUrl.ShouldReturn += field =>
            {
                field.ResignFirstResponder();
                return true;
            };
            View.Add(PhotoUrl);
        }

        private void BuildShareButton()
        {
            Share = UIButton.FromType(UIButtonType.RoundedRect);
            Share.Frame = new CGRect(View.Frame.X, View.Frame.Y + View.Frame.Height - 50, View.Frame.Width, 50);
            Share.SetTitle("Share Photo!", UIControlState.Normal);
            View.Add(Share);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ShareViewModel)value; }
        }

        public ShareViewModel ViewModel { get; set; }
    }
}