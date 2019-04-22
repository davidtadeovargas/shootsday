using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    public partial class RedSocial : ContentPage
    {
        public RedSocial()
        {
            init();
        }

        private void init()
        {
            InitializeComponent();

            BindingContext = new RedSocialViewModel(this);

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
            });

            
        }

        private void ViewProfileTapped(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            User user = (User)tappedEventArgs.Parameter;

            Profile_ Profile_ = new Profile_(user.id);            

            Navigation.PushModalAsync(Profile_);
        }

        private void ImageTapped(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            Picture Picture = (Picture)tappedEventArgs.Parameter;

            RedSocialDetail RedSocialDetail = new RedSocialDetail(Picture);

            Navigation.PushModalAsync(new MasterDetail(RedSocialDetail));
        }
    }
}