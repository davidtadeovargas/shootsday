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
    class RedSocialViewModel : BaseViewModel
    {
        private ObservableCollection<Picture> photoShoots_;
        private bool endOfRecords = false;
        
        public ObservableCollection<Picture> photoShoots
        {
            get
            {
                return photoShoots_;
            }
            set
            {
                photoShoots_ = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ViewProfileCommand { get; private set; }
        public ICommand ViewRedSocialDetailCommand { get; private set; }
        public Command ItemAppearingCommand
        {
            get;
            set;
        }
        



        public RedSocialViewModel(Page context) : base(context)
        {
            photoShoots = new ObservableCollection<Picture>();

            getPhotos();

            ItemAppearingCommand = new Command((args) => OnItemAppearing(args));
            ViewProfileCommand = new Command<User>(async (User) => await ViewProfile(User));
            ViewRedSocialDetailCommand = new Command<Picture>(async (Picture) => await ViewRedSocialDetail(Picture));
        }


        private async Task ViewProfile(User User)
        {
            Profile_ Profile_ = new Profile_(User.id);

            KeyboarClic(); //Simulate native clic sound             
            
            Navigation.PushAsync(new NavigationPage(new MasterDetail(Profile_)));
        }


        private async Task ViewRedSocialDetail(Picture Picture)
        {
            RedSocialDetail RedSocialDetail = new RedSocialDetail(Picture);

            KeyboarClic(); //Simulate native clic sound 
            
            Navigation.PushAsync(new MasterDetail(RedSocialDetail) {  });
        }


        private async Task OnItemAppearing(object args)
        {
            Picture Picture = (Picture)args;

            var picturesCount = photoShoots_.Count;

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
            var lastPictureId = photoShoots_[picturesCount - 1].id;
            if (Picture.id == lastPictureId)
            {
                getPhotos();
            }
        }

        private async void getPhotos()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            int id_event = SettingsManager.Instance.getIdEvent();

            try
            {
                LoadingManager.Instance.showLoading();

                var offset = 0;
                if (photoShoots_!=null && photoShoots_.Count() > 0)
                {
                    offset = photoShoots_.Count();
                }

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username }, limit = Constants.LIMIT, offset = offset });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.REDSOCIAL);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the photo list
                        List<Picture> photoshoots_ = jsonSystem.data.pictures;

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
