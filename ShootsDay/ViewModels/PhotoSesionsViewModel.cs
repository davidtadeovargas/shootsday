using System;
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
using ShootsDay.RequestModels;
using System.Windows.Input;
using DLToolkit.Forms.Controls;

namespace ShootsDay.ViewModels
{
    public class PhotoSesionsViewModel : BaseViewModel
    {

        private ObservableCollection<Photoshoot> photoShoots_;
        private bool endOfRecords = false;
        public ICommand ViewImageCommand { get; private set; }
        public FlowListView FlowListViewUsers { get; set; }
        public Label NoRecords { get; set; }
        public bool firstLoad { get; set; }

        private PhotoDetail PhotoDetail;
        private bool _userTapped;


        public PhotoSesionsViewModel(   Page context,
                                        FlowListView FlowListViewUsers_,
                                        Label NoRecords_) : base(context)
        {
            photoShoots = new ObservableCollection<Photoshoot>();

            firstLoad = true;

            FlowListViewUsers = FlowListViewUsers_;
            NoRecords = NoRecords_;

            getPhotos();

            ItemAppearingCommand = new Command((args) => OnItemAppearing(args));
            ViewImageCommand = new Command<Photoshoot>(async (Photoshoot) => await OnPhotoTappedAsync(Photoshoot));            
        }

        private async Task OnPhotoTappedAsync(Photoshoot Photoshoot)
        {
            if (_userTapped)
                return;

            _userTapped = true;
            
            KeyboarClic(); //Simulate native clic sound 

            PhotoDetail = new PhotoDetail(Photoshoot,this);
            await Navigation.PushAsync(new MasterDetail(PhotoDetail));            
        }

        public void resetUserTapped()
        {
            _userTapped = false;
        }

        private async Task OnItemAppearing(object args)
        {
            Photoshoot Photoshoot = (Photoshoot)args;

            var photoshootsCount = photoShoots_.Count;

            if (photoshootsCount == 0)
            {
                return;
            }

            //The server returned empty or lesser thant the limit
            if (endOfRecords)
            {
                return;
            }

            //If this is the end of the list get more records            
            var lastPhotoshootId = photoShoots_[photoshootsCount - 1].id;
            if (Photoshoot.id == lastPhotoshootId)
            {
                getPhotos();
            }
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


        public Command ItemAppearingCommand
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

                var offset = 0;
                if (photoShoots_.Count() > 0)
                {
                    offset = photoShoots_.Count();
                }

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username }, limit = Constants.LIMIT, offset = offset });
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

                        //When no records
                        if (photoshoots_.Count == 0 && firstLoad)
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

                        if (photoshoots_.Count() == 0 || photoshoots_.Count() < Constants.LIMIT)
                        {
                            endOfRecords = true;
                        }

                        foreach (var Photo in photoshoots_)
                        {
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
