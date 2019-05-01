using Foundation;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.RequestModels;
using ShootsDay.ViewModels;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace ShootsDay
{
    public partial class Invites : ContentPage
    {
        string url;
        string coordinates = "";



        public Invites()
        {
            init();   
        }

        private void init()
        {
            Title = "Invitación";
            InitializeComponent();
            get_Invites();

            var InvitesViewModel_ = new InvitesViewModel(this);
            BindingContext = InvitesViewModel_;

            //Load the image map
            Device.BeginInvokeOnMainThread(() =>
            {
                var urlMap = ImagesManager.Instance.getInvitationMap();
                mapPicture.Source = ImageSource.FromUri(new Uri(urlMap));
                //NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
            });
        }

        private async void get_Invites()
        {
            LoadingManager.Instance.showLoading();

            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username } });
                //var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
                var content = new StringContent(userData, Encoding.UTF8, "application/json");
                var uri = new Uri(Constants.EVENTS);
                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        address.Text = jsonSystem.data.Event.address;
                        addressSecond.Text = jsonSystem.data.Event.address_second;
                        coordinates = jsonSystem.data.Event.coordinates;
                    }
                    else
                    {
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(respuesta);
                    await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                LoadingManager.Instance.closeLoading();
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
            }
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            var uri = new Uri("http://maps.google.com");

            if (Device.OS == TargetPlatform.Android)
            {
                uri = new Uri("http://maps.google.com/?daddr=" + coordinates);                
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                uri = new Uri("http://maps.google.com/?daddr=" + coordinates);                
            }

            Device.OpenUri(uri);           
        }
    }
}
