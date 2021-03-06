﻿using DLToolkit.Forms.Controls;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoSesionsPage_ : ContentPage
    {
        public PhotoSesionsPage_()
        {
            init();
        }

        private void init()
        {
            InitializeComponent();            

            BindingContext = new PhotoSesionsViewModel(this, photosList, noRecords);

            Device.BeginInvokeOnMainThread(() => {
                //NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                tittleLabel.Text = SettingsManager.Instance.getTitleEvent();
                Title = "";
            });
        }
    }
}