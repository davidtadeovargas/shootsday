﻿using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using ShootsDay.Interfaces;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.Models.Share;
using ShootsDay.RequestModels;
using ShootsDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;

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
        private bool _userTapped;

        private PhotoSesionsViewModel PhotoSesionsViewModel;





        public PhotoDetail(Photoshoot Photoshoot, PhotoSesionsViewModel PhotoSesionsViewModel_)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent();

            this.PhotoSesionsViewModel = PhotoSesionsViewModel_;

            Photoshoot_ = Photoshoot; //Save current photo

            /*
                Init the view model
             */
            _photoDetailViewModel = new PhotoDetailViewModel(this);
            _photoDetailViewModel.photoshoot = Photoshoot_;
            BindingContext = _photoDetailViewModel; //Attach the binding context

            Device.BeginInvokeOnMainThread(() => {
                //NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                //Title = "Sesion de Fotos";
            });

            //Set the like
            var sourceLike = "http://shootsday.com.mx/imgs/liked.png";
            if (Photoshoot_.Like!=null)
            {
                sourceLike = "http://shootsday.com.mx/imgs/like.png";
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                likeImage.Source = sourceLike;
            });

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
        
        protected override void OnDisappearing()
        {
            this.PhotoSesionsViewModel.resetUserTapped(); //Prevent multi touch
        }

        private void OnLikeClicked(object sender, EventArgs e)
        {
            Like();
        }

        private void OnDownloadClicked(object sender, EventArgs e)
        {
            if (UtilsManager.Instance.isConectedToInternet(this)) //If connectivity error display message to user
            {
                _photoDetailViewModel.DownloadCommand();
                //downloader.DownloadFile(Photoshoot_.url_image, "XF_Downloads");
            }
        }

        private async void Like()
        {
            if (_userTapped)
                return;

            _userTapped = true;

            if (UtilsManager.Instance.isConectedToInternet(this)) //If connectivity error display message to user
            {
                try
                {
                    string username = SettingsManager.Instance.getUserName();
                    string password = SettingsManager.Instance.getPassword();
                    int user_id = SettingsManager.Instance.getUserId();

                    int value;
                    int like_id;
                    if (Photoshoot_.Like == null)
                    {
                        value = 1;
                        like_id = -1;
                    }
                    else
                    {
                        value = -1;
                        like_id = Photoshoot_.Like.id;
                    }

                    var client = new HttpClient();
                    var userData = Newtonsoft.Json.JsonConvert.SerializeObject(
                        new
                        {
                            Like = new { id = like_id, photoshoot_id = Photoshoot_.id, value = value },
                            Login = new { password = password, username = username, user_id = user_id }
                        });

                    var content = new StringContent(userData, Encoding.UTF8, "application/json");
                    var uri = new Uri(Constants.LIKE);

                    var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                    if (result.IsSuccessStatusCode)
                    {
                        var tokenJson = await result.Content.ReadAsStringAsync();
                        var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestLike>(tokenJson);

                        if (jsonSystem.status.type != "error")
                        {

                            //Set the like
                            string sourceLike;
                            if (Photoshoot_.Like == null)
                            {
                                Photoshoot_.Like = jsonSystem.data.Like;
                                sourceLike = "http://shootsday.com.mx/imgs/like.png";
                            }
                            else
                            {
                                Photoshoot_.Like = null;
                                sourceLike = "http://shootsday.com.mx/imgs/liked.png";
                            }


                            Device.BeginInvokeOnMainThread(() =>
                            {
                                likeImage.Source = sourceLike;
                            });
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
                    Debug.WriteLine("Excepcion: " + ex.Message);
                    await DisplayAlert("", ex.Message, "Aceptar");
                }
            }            

            _userTapped = false;
        }
    }
}