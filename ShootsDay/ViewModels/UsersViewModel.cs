using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.ViewModels
{
    class UsersViewModel : BaseViewModel
    {
        private ObservableCollection<User> users_;


        public UsersViewModel(Page context) : base(context)
        {
            users = new ObservableCollection<User>();

            getUsers();            
        }        

        private async Task OnUserTappedAsync(object args)
        {
            User User = (User)args;            
        }


        public ObservableCollection<User> users
        {
            get
            {
                return users_;
            }
            set
            {
                users_ = value;
                RaisePropertyChanged();
            }
        }

        private async void getUsers()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            int id_event = SettingsManager.Instance.getIdEvent();
            int user_id = SettingsManager.Instance.getUserId();

            try
            {
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username, user_id = user_id } });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.EVENT_USERS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the users list
                        List<User> users_ = jsonSystem.data.Users;
                        foreach (var User in users_)
                        {
                            User.url_image = ImagesManager.Instance.getProfilePicture(User.id);
                            User.name = User.name + " " + User.lastname;

                            Device.BeginInvokeOnMainThread(() => {
                                users.Add(User);
                            });
                        }
                    }
                    else
                    {
                        LoadingManager.Instance.closeLoading();

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
