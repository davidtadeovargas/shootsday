using Android.Content;
using Android.Graphics;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.Models.Share;
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
        ShareImageViewModel _shareImageViewModel;




        public PhotoDetail(Photoshoot Photoshoot)
        {
            InitializeComponent();

            Photoshoot_ = Photoshoot; //Save current photo

            /*
                Init the model for the share image
             */
            _shareImageViewModel = new ShareImageViewModel();            
            BindingContext = _shareImageViewModel; //Attach the binding context

            /*
                Load the title and image
             */
            title.Text = Photoshoot_.description;
            Device.BeginInvokeOnMainThread(() =>
            {
                string url = Photoshoot_.url_image;
                image.Source = ImageSource.FromUri(new Uri(url));

                _shareImageViewModel.Source = image.Source;//For binding
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