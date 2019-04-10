﻿using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using ShootsDay.Interfaces;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.Models.Share;
using ShootsDay.RequestModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
            Like();
        }


        private async void Like()
        {
            try
            {
                LoadingManager.Instance.showLoading();

                string username = SettingsManager.Instance.getUserName();
                string password = SettingsManager.Instance.getPassword();
                int user_id = SettingsManager.Instance.getUserId();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        Like = new { photoshoot_id = Photoshoot_.id },
                        Login = new { password = password, username = username, user_id = user_id }
                    });

                var content = new StringContent(userData, Encoding.UTF8, "application/json");
                var uri = new Uri(Constants.LIKE_ADD);                

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestLike>(tokenJson);

                    if (jsonSystem.status.type != "error")
                    {
                        
                    }
                    else
                    {
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(respuesta);
                    await DisplayAlert("", respuesta, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                LoadingManager.Instance.closeLoading();                
                Debug.WriteLine("Excepcion: " + ex.Message);
                await DisplayAlert("", ex.Message, "Aceptar");
            }            
        }
    }
}