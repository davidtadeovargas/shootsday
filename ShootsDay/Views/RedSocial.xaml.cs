﻿using ShootsDay.Managers;
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

        private void ViewProfileTapped(object sender, TappedEventArgs e)
        {
            User user = (User)e.Parameter;

            Profile_ Profile_ = new Profile_(user.id);            

            Navigation.PushModalAsync(Profile_);
        }

        private void ImageTapped(object sender, TappedEventArgs e)
        {
            Picture Picture = (Picture)e.Parameter;

            RedSocialDetail RedSocialDetail = new RedSocialDetail(Picture);

            Navigation.PushModalAsync(new MasterDetail(RedSocialDetail));
        }
    }
}