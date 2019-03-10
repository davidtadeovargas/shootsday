using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShootsDay.Models;
using Xamarin.Forms.Platform.Android;

namespace ShootsDay.Droid
{
    /// <summary>
	/// This adapter uses a view defined in /Resources/Layout/NativeAndroidListViewCell.axml
	/// as the cell layout
	/// </summary>
	public class NativeAndroidListViewAdapter : BaseAdapter<Photoshoot>
    {
        readonly Activity context;
        IList<Photoshoot> tableItems = new List<Photoshoot>();

        public IEnumerable<Photoshoot> Items
        {
            set
            {
                tableItems = value.ToList();
            }
        }

        public NativeAndroidListViewAdapter(Activity context, PhotoSesionListView view)
        {
            this.context = context;
            tableItems = view.Items.ToList();
        }

        public override Photoshoot this[int position]
        {
            get
            {
                return tableItems[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get {
                float counter = tableItems.Count / 2;
                int counter_ = counter % 3 == 0 ? (int)counter + 1 : (int)counter;
                return tableItems.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.NativeAndroidListViewCelll, null);
            }

            if (position > 0)
            {
                position = position + 1;
            }

            if (position < tableItems.Count)
            {
                var item = tableItems[position];

                string server = "http://shootsday.com.mx";
                string urlImage = server + item.url_image;

                // grab the old image and dispose of it
                if (view.FindViewById<ImageView>(Resource.Id.ImageLeft).Drawable != null)
                {
                    using (var image = view.FindViewById<ImageView>(Resource.Id.ImageLeft).Drawable as BitmapDrawable)
                    {
                        if (image != null)
                        {
                            if (image.Bitmap != null)
                            {
                                //image.Bitmap.Recycle ();
                                image.Bitmap.Dispose();
                            }
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(urlImage))
                {
                    var imageBitmap = GetImageBitmapFromUrl(urlImage);
                    var imageLeft = view.FindViewById<ImageView>(Resource.Id.ImageLeft);
                    imageLeft.SetImageBitmap(imageBitmap);
                }
                else
                {
                    // clear the image
                    view.FindViewById<ImageView>(Resource.Id.ImageLeft).SetImageBitmap(null);
                }


                if ((position + 1) < tableItems.Count)
                {
                    var itemR = tableItems[position + 1];
                    urlImage = server + itemR.url_image;

                    // grab the old image and dispose of it
                    if (view.FindViewById<ImageView>(Resource.Id.ImageRight).Drawable != null)
                    {
                        using (var image = view.FindViewById<ImageView>(Resource.Id.ImageRight).Drawable as BitmapDrawable)
                        {
                            if (image != null)
                            {
                                if (image.Bitmap != null)
                                {
                                    //image.Bitmap.Recycle ();
                                    image.Bitmap.Dispose();
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(urlImage))
                    {
                        var imageBitmap = GetImageBitmapFromUrl(urlImage);
                        var imageRight = view.FindViewById<ImageView>(Resource.Id.ImageRight);
                        imageRight.SetImageBitmap(imageBitmap);
                    }
                    else
                    {
                        // clear the image
                        view.FindViewById<ImageView>(Resource.Id.ImageRight).SetImageBitmap(null);
                    }
                }
            }

            return view;
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
    }
}