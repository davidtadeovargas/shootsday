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



        public Profile_(int userId_)
        {
            init(userId_);
        }


        public Profile_()
        {
            init(-1);
        }

        private void init(int userId_)
        {
            InitializeComponent();

            Title = "Perfil de usuario";

            userId = userId_;

            get_profile();
        }


        private async void get_profile()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            int id_event = SettingsManager.Instance.getIdEvent();

            if (userId==-1)
            {
                userId = SettingsManager.Instance.getUserId();
            }
            
            try
            {
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Event = new { id = id_event },
                    Login = new { password = password, username = username }
                });
                //var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
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
            contacts.Text = profile.Event.count_users.ToString() + " contactos";            
        }
    }
}