﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ShootsDay.Models;
using Xamarin.Forms;
using System.Net.Http;
using ShootsDay.Views;
using ShootsDay.Managers;

namespace ShootsDay.ViewModels
{
    class PhotoSesionsViewModel : BaseViewModel
    {

        private ObservableCollection<Photoshoot> photoShoots_;

        


        public PhotoSesionsViewModel(Page context) : base(context)
        {
            photoShoots = new ObservableCollection<Photoshoot>();

            getPhotos();

            ItemTappedCommand = new Command((args) => OnPhotoTappedAsync(args));
        }

        private async Task OnPhotoTappedAsync(object args)
        {
            Photoshoot photoshoot = (Photoshoot)args;
            await Navigation.PushModalAsync(new MasterDetail(new PhotoDetail(photoshoot)));
        }

        public ObservableCollection<Photoshoot> photoShoots
        {
            get {
                return photoShoots_;
            }
            set {
                photoShoots_ = value;
                RaisePropertyChanged();
            }
        }        

        public Command ItemTappedCommand
        {
            get;
            set;
        }

        private async void getPhotos()
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();

            try
            {
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username } });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.PHOTOSHOOTS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the photo list
                        List<Photoshoot> photoshoots_ = jsonSystem.data.Photoshoots;
                        foreach (var Photo in photoshoots_)
                        {
                            Photo.url_image = ImagesManager.Instance.gePhotoshootDetailImage(Photo.url_image);
                            Device.BeginInvokeOnMainThread(() => {
                                photoShoots.Add(Photo);
                            });
                        }
                    }
                    else
                    {
                        Alert.DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(respuesta);
                }
            }
            catch (Exception ex)
            {
                Alert.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}