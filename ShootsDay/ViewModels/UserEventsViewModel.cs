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
using Xamarin.Forms;

namespace ShootsDay.ViewModels
{
    class UserEventsViewModel : BaseViewModel
    {
        private ObservableCollection<Event> events_;


        public UserEventsViewModel(Page context) : base(context)
        {
            events = new ObservableCollection<Event>();            

            getUserEvents();

            ItemTappedCommand = new Command((args) => OnItemTappedAsync(args));
        }

        private async Task OnItemTappedAsync(object args)
        {
            Event Event = (Event)args;

            //Save the event in ram
            SettingsManager.Instance.setIdEvent(Event.id);
            SettingsManager.Instance.setTitleEvent(Event.title);
            SettingsManager.Instance.setEventCode(Event.code);
            SettingsManager.Instance.SavePropertiesAsync();

            //Open the main window
            Navigation.PushModalAsync(new MasterDetail(new Home_()));
        }

        public Command ItemTappedCommand
        {
            get;
            set;
        }

        public ObservableCollection<Event> events
        {
            get
            {
                return events_;
            }
            set
            {
                events_ = value;
                RaisePropertyChanged();
            }
        }

        private async void getUserEvents()
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

                var uri = new Uri(Constants.USER_EVENTS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the events list
                        List<Event> events_ = jsonSystem.data.Events;
                        foreach (var Event in events_)
                        {                            
                            Device.BeginInvokeOnMainThread(() => {
                                events.Add(Event);
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
                    Alert.DisplayAlert("Error", respuesta, "Aceptar");
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(respuesta);
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
