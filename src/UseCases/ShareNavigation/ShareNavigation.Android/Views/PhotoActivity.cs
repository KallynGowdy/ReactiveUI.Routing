using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ReactiveUI.Routing.Android;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity(Label = "Photo!")]
    public class PhotoActivity : RoutableActivity<PhotoViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PhotoListItem);
            this.WhenActivated(d =>
            {
                d(ViewModel.WhenAnyValue(vm => vm.PhotoData)
                    .Where(p => p != null)
                    .Select(data => BitmapFactory.DecodeByteArray(data, 0, data.Length))
                    .Do(b => Photo.SetImageBitmap(b))
                    .Subscribe());
            });
        }

        private ImageView Photo => this.GetControl<ImageView>();

    }
}