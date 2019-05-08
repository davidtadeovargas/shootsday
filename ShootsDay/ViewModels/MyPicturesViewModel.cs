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
using Xamarin.Forms;

namespace ShootsDay.ViewModels
{
    class MyPicturesViewModel : BaseViewModel
    {
        private ObservableCollection<Picture> pictures_;
        private bool endOfRecords = false;

        public ObservableCollection<Picture> pictures
        {
            get
            {
                return pictures_;
            }
            set
            {
                pictures_ = value;
                RaisePropertyChanged();
            }
        }
        public FlowListView FlowListViewUsers { get; set; }
        public StackLayout NoRecords { get; set; }
        public bool firstLoad { get; set; }




        public MyPicturesViewModel( Page context,
                                    FlowListView FlowListViewUsers_,
                                    StackLayout NoRecords_) : base(context)
        {
            pictures = new ObservableCollection<Picture>();

            FlowListViewUsers = FlowListViewUsers_;
            NoRecords = NoRecords_;

            firstLoad = true;

            getPictures();

            ItemAppearingCommand = new Command((args) => OnItemAppearing(args));
        }

        public Command ItemAppearingCommand
        {
            get;
            set;
        }

        private async Task OnItemAppearing(object args)
        {
            Picture Picture = (Picture)args;

            var picturesCount = pictures_.Count;

            if (picturesCount == 0)
            {
                return;
            }

            //The server returned empty or lesser thant the limit
            if (endOfRecords)
            {
                return;
            }

            //If this is the end of the list get more records            
            var lastPictureId = pictures_[picturesCount - 1].id;
            if (Picture.id == lastPictureId)
            {
                getPictures();
            }
        }

        private async void getPictures()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            int id_event = SettingsManager.Instance.getIdEvent();

            try
            {
                LoadingManager.Instance.showLoading();

                var offset = 0;
                if (pictures_!=null && pictures_.Count() > 0)
                {
                    offset = pictures_.Count();
                }

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username }, limit = Constants.LIMIT, offset = offset });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.MY_PICTURES);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the photo list
                        List<Picture> pictures_ = jsonSystem.data==null? new List<Picture>(): jsonSystem.data.pictures;

                        //When no records
                        if (pictures_.Count == 0 && firstLoad)
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

                        if (pictures_.Count() == 0 || pictures_.Count() < Constants.LIMIT)
                        {
                            endOfRecords = true;
                        }

                        foreach (var Picture in pictures_)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                pictures.Add(Picture);
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
