using System;
using System.Drawing;
using System.Linq;
using CoreFoundation;
using CoreGraphics;
using UIKit;
using Foundation;
using ReactiveUI;
using ShareNavigation.ViewModels;
using System.Reactive.Linq;

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
    public class PhotoListViewController : UIViewController, IViewFor<PhotoListViewModel>, IUICollectionViewDataSource, IUICollectionViewDelegate
    {
        private UIButton share;
        private UICollectionView photos;
        private UIImage[] images = new UIImage[0];

        public UIImage[] Images
        {
            get { return images; }
            set
            {
                images = value;
                photos.ReloadData();
            }
        }

        public PhotoListViewController()
        {
            this.WhenActivated(d =>
            {
                d(ViewModel.WhenAnyValue(vm => vm.LoadedPhotoData)
                    .Where(data => data != null)
                    .Select(data => data.Select(p => new UIImage(NSData.FromArray(p))))
                    .Subscribe(i => Images = i.ToArray()));
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.share));
                ViewModel.LoadPhotos.Execute(null);
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
            var layout = new UICollectionViewFlowLayout
            {
                EstimatedItemSize = new CGSize(75, 75),
                MinimumInteritemSpacing = 5,
                ItemSize = new CGSize(75, 75)
            };
            photos = new UICollectionView(new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height - 50),
                layout)
            {
                DataSource = this,
                Delegate = this
            };
            photos.RegisterClassForCell(typeof(UICollectionViewCell), "Cell");
            photos.BackgroundColor = UIColor.White;
            share = UIButton.FromType(UIButtonType.System);
            share.Frame = new CGRect(View.Frame.X, View.Frame.Y + View.Frame.Height - 50, View.Frame.Width, 50);
            share.SetTitle("Share!", UIControlState.Normal);
            View = new UniversalView()
            {
                photos,
                share
            };
            View.BackgroundColor = UIColor.White;
            Title = "Photos!";

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PhotoListViewModel)value; }
        }

        public PhotoListViewModel ViewModel { get; set; }

        public nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Images.Length;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var image = Images[indexPath.Row];
            UICollectionViewCell cell = (UICollectionViewCell)collectionView.DequeueReusableCell("Cell", indexPath);
            cell.ContentView.Add(new UIImageView(image)
            {
                Frame = new CGRect(0, 0, 75, 75),
                ContentMode = UIViewContentMode.ScaleAspectFill
            });
            return cell;
        }
    }
}