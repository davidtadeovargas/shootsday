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
    public partial class RedSocialDetail : ContentPage
    {
        Picture Picture_;
        RedSocialDetailViewModel RedSocialDetailViewModel;




        public RedSocialDetail(Picture Picture)
        {
            InitializeComponent();

            Picture_ = Picture; //Save current photo

            /*
                Init the view model
             */
            RedSocialDetailViewModel = new RedSocialDetailViewModel();
            RedSocialDetailViewModel.Picture = Picture_;
            BindingContext = RedSocialDetailViewModel; //Attach the binding context

            /*
                Load the title and image
             */
            title.Text = Picture_.description;
            Device.BeginInvokeOnMainThread(() =>
            {
                string url = Picture_.url_image;
                image.Source = ImageSource.FromUri(new Uri(url));

                RedSocialDetailViewModel.Source = image.Source;//For binding

                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "Red Social";                
            });
        }

        private void OnSendClicked(object sender, EventArgs e)
        {
            
        }

        private async Task OnItemClicked(object sender, EventArgs e)
        {
            Picture Picture = (Picture)sender;

            
        }
    }
}