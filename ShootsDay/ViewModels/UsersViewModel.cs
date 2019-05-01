using DLToolkit.Forms.Controls;
using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.RequestModels;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShootsDay.ViewModels
{
    class UsersViewModel : BaseViewModel
    {
        private ObservableCollection<User> users_;
        private bool endOfRecords = false;
        public ICommand ViewPerfilCommand { get; private set; }
        public FlowListView FlowListViewUsers { get; set; }
        public StackLayout NoRecords { get; set; }
        public bool firstLoad { get; set; }




        public UsersViewModel(  Page context,
                                FlowListView FlowListViewUsers_,
                                StackLayout NoRecords_) : base(context)
        {
            users = new ObservableCollection<User>();

            FlowListViewUsers = FlowListViewUsers_;
            NoRecords = NoRecords_;

            firstLoad = true;

            getUsers();

            ItemTappedCommand = new Command((args) => OnUserTappedAsync(args));
            ItemAppearingCommand = new Command((args) => OnItemAppearing(args));
            ViewPerfilCommand = new Command<User>(async (User) => await OnViewProfileTappedAsync(User));
        }


        private async Task OnViewProfileTappedAsync(User User)
        {
            Profile_ Profile_ = new Profile_(User.id);

            KeyboarClic(); //Simulate native clic sound 

            Navigation.PushModalAsync(new MasterDetail(Profile_));
        }

        private async Task OnUserTappedAsync(object args)
        {
            User User = (User)args;

            Profile_ Profile_ = new Profile_(User.id);

            Navigation.PushModalAsync(Profile_);            
        }

        private async Task OnItemAppearing(object args)
        {
            User User = (User)args;

            var usersCount = users_.Count;

            if (usersCount == 0)
            {
                return;
            }

            //The server returned empty or lesser thant the limit
            if (endOfRecords)
            {
                return;
            }

            //If this is the end of the list get more records            
            var lastUserId = users_[usersCount - 1].id;
            if (User.id == lastUserId)
            {
                getUsers();
            }
        }

        public Command ItemTappedCommand
        {
            get;
            set;
        }
        public Command ItemAppearingCommand
        {
            get;
            set;
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

                var offset = 0;
                if (users_.Count()>0)
                {
                    offset =  users_.Count();
                }

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username, user_id = user_id}, limit = Constants.LIMIT, offset = offset });
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

                        //When no records
                        if (users_.Count==0 && firstLoad)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                FlowListViewUsers.IsVisible = false;
                                NoRecords.IsVisible = true;
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                FlowListViewUsers.IsVisible = true;
                                NoRecords.IsVisible = false;
                            });
                        }

                        firstLoad = false;

                        if (users_.Count()==0 || users_.Count()<Constants.LIMIT)
                        {
                            endOfRecords = true;
                        }

                        foreach (var User in users_)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                users.Add(User);
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

                    Alert.DisplayAlert("Error", respuesta, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                LoadingManager.Instance.closeLoading();
                Alert.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
