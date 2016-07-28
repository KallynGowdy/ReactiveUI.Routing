using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity(Label = "Share!")]
    public class PhotoActivity : RoutableActivity<PhotoViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PhotoListItem);
            this.WhenActivated(d =>
            {
                d(ViewModel.WhenAnyValue(vm => vm.PhotoUrl)
                    .Where(p => !string.IsNullOrEmpty(p))
                    .SelectMany(PhotoListItemAdapter.GetImageBitmapFromUrl)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(b => Photo.SetImageBitmap(b))
                    .Subscribe());
            });
        }

        private ImageView Photo => this.GetControl<ImageView>();

    }
}