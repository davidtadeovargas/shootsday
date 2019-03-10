using System.Linq;
using ShootsDay;
using ShootsDay.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(PhotoSesionListView), typeof(NativeAndroidListViewRenderer))]
namespace ShootsDay.Droid
{
    public class NativeAndroidListViewRenderer : ListViewRenderer
    {
        Context _context = Forms.Context;


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // unsubscribe
                Control.ItemClick -= OnItemClick;
            }

            if (e.NewElement != null)
            {
                // subscribe
                Control.Adapter = new NativeAndroidListViewAdapter(_context as Android.App.Activity, e.NewElement as PhotoSesionListView);
                Control.ItemClick += OnItemClick;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == PhotoSesionListView.ItemsProperty.PropertyName)
            {
                Control.Adapter = new NativeAndroidListViewAdapter(_context as Android.App.Activity, Element as PhotoSesionListView);
            }
        }

        void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            ((PhotoSesionListView)Element).NotifyItemSelected(((PhotoSesionListView)Element).Items.ToList()[e.Position - 1]);
        }
    }
}