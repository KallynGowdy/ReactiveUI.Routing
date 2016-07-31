using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    public class PhotoListItemAdapter : BaseAdapter<Bitmap>
    {
        private readonly Activity context;
        private readonly PhotoListViewModel viewModel;
        private Bitmap[] bitmaps = new Bitmap[0];

        public PhotoListItemAdapter(Activity context, PhotoListViewModel viewModel)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            this.context = context;
            this.viewModel = viewModel;
            this.viewModel.WhenAnyValue(vm => vm.LoadedPhotoData)
                .Where(data => data != null)
                .Select(photos => photos.Select(p => BitmapFactory.DecodeByteArray(p, 0, p.Length)).ToArray())
                .Do(bitmaps => this.bitmaps = bitmaps)
                .Do(b => this.NotifyDataSetChanged())
                .Subscribe();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.PhotoListItem, null);
            var item = bitmaps[position];
            view.FindViewById<ImageView>(Resource.Id.photo).SetImageBitmap(item);
            return view;
        }

        public override int Count => bitmaps.Length;
        public override Bitmap this[int position] => bitmaps[position];
    }
}