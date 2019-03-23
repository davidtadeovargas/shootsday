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




        public Profile_()
        {
            InitializeComponent();

            Title = "Perfil de usuario";
            get_profile();
        }


        private async void get_profile()
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            string user_id = Application.Current.Properties["user_id"].ToString();            
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
                var uri = new Uri(Constants.USERS_PROFILE + Convert.ToInt32(user_id) + ".json");
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
            if (!string.IsNullOrEmpty(profile.User.url_image))
            {
                var url = ImagesManager.Instance.getProfilePicture(profile.User.id);
                Device.BeginInvokeOnMainThread(() =>
                {
                    profile_img.Source = ImageSource.FromUri(new Uri(url));
                });                                
            }

            name.Text = profile.User.name + " " + profile.User.lastname;
            event_.Text = profile.Event.title;
            contacts.Text = profile.Event.count_users.ToString() + " contactos";            
        }
    }
}