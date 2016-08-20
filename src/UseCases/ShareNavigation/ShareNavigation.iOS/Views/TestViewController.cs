using System;
using System.Drawing;

using CoreFoundation;
using CoreGraphics;
using UIKit;
using Foundation;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.iOS.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("TestViewController")]
    public class TestViewController : UIViewController, IViewFor<PhotoListViewModel>
    {
        private UILabel label;
        private UIButton share;

        public TestViewController()
        {
            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.share));
            });
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            label = new UILabel()
            {
                Text = "Hello, iOS!",
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.Black,
                Frame = new CGRect(20, 200, 280, 44)
            };
            share = UIButton.FromType(UIButtonType.System);
            share.Frame = new CGRect(20, 300, 100, 44);
            share.SetTitle("Share!", UIControlState.Normal);
            View = new UniversalView()
            {
                label,
                share
            };
            View.BackgroundColor = UIColor.White;
            Title = "Test";

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PhotoListViewModel)value; }
        }

        public PhotoListViewModel ViewModel { get; set; }
    }
}