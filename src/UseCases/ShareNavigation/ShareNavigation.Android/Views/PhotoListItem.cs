using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShareNavigation.Views
{
    public class PhotoListItemAdapter : BaseAdapter<Photo>
    {
        private readonly Activity context;
        private readonly Photo[] items;

        public PhotoListItemAdapter(Activity context, Photo[] items)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (items == null) throw new ArgumentNullException(nameof(items));
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.PhotoListItem, null);
            var item = items[position];
            var bitmap = GetImageBitmapFromUrl(item.PhotoUrl);
            view.FindViewById<ImageView>(Resource.Id.photo).SetImageBitmap(bitmap);
            return view;
        }

        public override int Count => items.Length;
        public override Photo this[int position] => items[position];
    }
}