using ShootsDay.Models;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace ShootsDay
{
    public partial class Invites : ContentPage
    {
        string url;
        public Invites()
        {
            Title = "Invitación";
            InitializeComponent();
            get_Invites();
        }

        private async void get_Invites()
        {
            Loading.Instance.showLoading();

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

                Loading.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        address.Text = jsonSystem.data.Event.address;
                        addressSecond.Text = jsonSystem.data.Event.address_second;
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
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
            }
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
        }
    }
}
