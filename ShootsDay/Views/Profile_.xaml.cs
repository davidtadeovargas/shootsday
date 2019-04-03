using ShootsDay.Managers;
using ShootsDay.Models;
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
    public partial class Profile_ : ContentPage
    {
        string host;

        public int userId { get; set; }
        public int eventId { get; set; }
        public bool anotherProfile { get; set; }



        public Profile_(int userId_)
        {
            init(userId_, -1,true);
        }


        public Profile_()
        {
            init(-1,-1,false);
        }

        private void init(int userId_, int eventId_, bool anotherProfile_)
        {
            InitializeComponent();

            Title = "Perfil de usuario";

            userId = userId_;
            eventId = eventId_;
            anotherProfile = anotherProfile_;

            get_profile();

            //Do not show the users total when it is another profile and not the current
            if (anotherProfile)
            {
                contacts.IsVisible = false;
            }
        }


        private async void get_profile()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();            

            if (userId==-1)
            {
                userId = SettingsManager.Instance.getUserId();
            }

            if (eventId == -1)
            {
                eventId = SettingsManager.Instance.getIdEvent();
            }

            try
            {
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    id = userId,
                    Event = new { id = eventId },
                    Login = new { password = password, username = username }
                });
                
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var url = ImagesManager.Instance.getProfilePicture(userId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    profile_img.Source = ImageSource.FromUri(new Uri(url));
                });

                var currentUserId = SettingsManager.Instance.getUserId();
                var uri = new Uri(Constants.USERS_PROFILE + Convert.ToInt32(currentUserId) + ".json");
                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        set_data_profile(jsonSystem.data);
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


        private void set_data_profile(Data profile)
        {
            name.Text = profile.User.name + " " + profile.User.lastname;
            event_.Text = profile.Event.title;
            contacts.Text = "Contactos en este evento: " + profile.Event.count_users.ToString();
        }
    }
}