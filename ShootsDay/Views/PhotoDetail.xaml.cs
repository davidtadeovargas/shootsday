using Android.Content;
using Android.Graphics;
using ShootsDay.Managers;
using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoDetail : ContentPage
    {
        Photoshoot Photoshoot_;




        public PhotoDetail(Photoshoot Photoshoot)
        {
            InitializeComponent();

            Photoshoot_ = Photoshoot; //Save current photo

            /*
                Load the title and image
             */
            title.Text = Photoshoot_.description;
            Device.BeginInvokeOnMainThread(() =>
            {
                string url = ImagesManager.Instance.gePhotoshootDetailImage(Photoshoot_.url_image);
                image.Source = ImageSource.FromUri(new Uri(url));
            });
        }

        private void OnDownloadClicked(object sender, EventArgs e)
        {

        }

        
        private void OnLikeClicked(object sender, EventArgs e)
        {

        }

        private void OnShareClicked(object sender, EventArgs e)
        {

        }
    }
}