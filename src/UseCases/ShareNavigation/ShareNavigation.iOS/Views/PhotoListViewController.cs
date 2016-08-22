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
    [Register(nameof(PhotoListViewController))]
    public class PhotoListViewController : UIViewController, IViewFor<PhotoListViewModel>, IUICollectionViewDataSource
    {
        private class CollectionViewDelegate : UICollectionViewDelegate
        {
            private readonly PhotoListViewController controller;

            public CollectionViewDelegate(PhotoListViewController controller)
            {
                this.controller = controller;
            }

            public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
            {
                controller.ViewModel.ShowPhoto.Execute(indexPath.Row);
            }
        }

        public UIButton Share { get; private set; }
        public UICollectionView Photos { get; private set; }
        private UIImage[] images = new UIImage[0];

        public UIImage[] Images
        {
            get { return images; }
            set
            {
                images = value;
                Photos.ReloadData();
            }
        }

        public PhotoListViewController()
        {
            this.WhenActivated(d =>
            {
                d(ViewModel.WhenAnyValue(vm => vm.LoadedPhotoData)
                    .Where(data => data != null)
                    .Select(data => data.Select(p => new UIImage(NSData.FromArray(p))))
                    .Do(i => Images = i.ToArray())
                    .Do(i => Photos.ReloadData())
                    .Subscribe());
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.Share));
                ViewModel.LoadPhotos.Execute(null);
            });
        }

        public override void ViewDidLoad()
        {
            BuildPhotosList();
            BuildShareButton();
            View.Add(Photos);
            View.Add(Share);
            View.BackgroundColor = UIColor.White;
            Title = "Photos!";

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        private void BuildShareButton()
        {
            Share = UIButton.FromType(UIButtonType.System);
            Share.Frame = new CGRect(View.Frame.X, View.Frame.Y + View.Frame.Height - 50, View.Frame.Width, 50);
            Share.SetTitle("Share!", UIControlState.Normal);
        }

        private void BuildPhotosList()
        {
            var layout = new UICollectionViewFlowLayout
            {
                EstimatedItemSize = new CGSize(75, 75),
                MinimumInteritemSpacing = 5,
                ItemSize = new CGSize(75, 75)
            };
            Photos =
                new UICollectionView(new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height - 50),
                    layout)
                {
                    DataSource = this,
                    Delegate = new CollectionViewDelegate(this),
                    AllowsSelection = true
                };
            Photos.RegisterClassForCell(typeof(UICollectionViewCell), "Cell");
            Photos.BackgroundColor = UIColor.White;
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