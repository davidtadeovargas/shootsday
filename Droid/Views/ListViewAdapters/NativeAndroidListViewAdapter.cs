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
using ShootsDay.Models.Views;
using Xamarin.Forms.Platform.Android;

namespace ShootsDay.Droid
{
    /// <summary>
	/// This adapter uses a view defined in /Resources/Layout/NativeAndroidListViewCell.axml
	/// as the cell layout
	/// </summary>
	public class NativeAndroidListViewAdapter : BaseAdapter<PhotoSesionListViewModel>
    {
        readonly Activity context;
        IList<PhotoSesionListViewModel> tableItems = new List<PhotoSesionListViewModel>();

        public IEnumerable<PhotoSesionListViewModel> Items
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

        public override PhotoSesionListViewModel this[int position]
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

            var item = tableItems[position]; //Get the item

            //Create the urls for the images
            string server = "http://shootsday.com.mx";
            string urlImageLeft = server + item.photoshootLeft.url_image;
            string urlImageRigth = server + item.photoshootRigth.url_image;

            // grab the old image and dispose of it LEFT
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
            if (!String.IsNullOrWhiteSpace(urlImageLeft)) //If there is not url so do not parse image to bitmap
            {
                var imageBitmap = GetImageBitmapFromUrl(urlImageLeft);
                var imageLeft = view.FindViewById<ImageView>(Resource.Id.ImageLeft);
                imageLeft.SetImageBitmap(imageBitmap);
            }
            else
            {
                // clear the image
                view.FindViewById<ImageView>(Resource.Id.ImageLeft).SetImageBitmap(null);
            }

            // grab the old image and dispose of it RIGHT
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
            if (!String.IsNullOrWhiteSpace(urlImageRigth)) //If there is not url so do not parse image to bitmap
            {
                var imageBitmap = GetImageBitmapFromUrl(urlImageRigth);
                var imageRight = view.FindViewById<ImageView>(Resource.Id.ImageRight);
                imageRight.SetImageBitmap(imageBitmap);
            }
            else
            {
                // clear the image
                view.FindViewById<ImageView>(Resource.Id.ImageRight).SetImageBitmap(null);
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