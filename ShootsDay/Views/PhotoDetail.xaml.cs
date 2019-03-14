using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using ShootsDay.Interfaces;
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
        PhotoDetailViewModel _photoDetailViewModel;        

        /*
            Permissions needed
             */
        readonly string[] PermissionsLocation =
        {
          Manifest.Permission.ReadExternalStorage,
          Manifest.Permission.WriteExternalStorage
        };

        const int RequestShareId = 0;




        public PhotoDetail(Photoshoot Photoshoot)
        {
            InitializeComponent();

            Photoshoot_ = Photoshoot; //Save current photo

            /*
                Init the view model
             */
            _photoDetailViewModel = new PhotoDetailViewModel();
            _photoDetailViewModel.photoshoot = Photoshoot_;
            BindingContext = _photoDetailViewModel; //Attach the binding context

            /*
                Load the title and image
             */
            title.Text = Photoshoot_.description;
            Device.BeginInvokeOnMainThread(() =>
            {
                string url = Photoshoot_.url_image;
                image.Source = ImageSource.FromUri(new Uri(url));

                _photoDetailViewModel.Source = image.Source;//For binding
            });            
        }

        private void OnLikeClicked(object sender, EventArgs e)
        {

        }        
    }
}