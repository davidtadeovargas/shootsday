using ShootsDay.Models;
using ShootsDay.Models.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Views
{
    public class PhotoSesionsPage : ContentPage
    {
        PhotoSesionListView nativeListView;
        List<PhotoSesionListViewModel> photoSesionListViewModels; //List view model for the listview of photos




        public PhotoSesionsPage()
        {
            getPhotos();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Photoshoot modelSelected = (Photoshoot)e.SelectedItem;
            await Navigation.PushModalAsync(new MasterDetail(new PhotoDetail(modelSelected)));
        }


        private void init()
        {
            nativeListView = new PhotoSesionListView
            {
                Items = photoSesionListViewModels
            };

            Content = new StackLayout
            {
                Children = {    new Label { Text = Application.Current.Properties["title_event"].ToString(),
                                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                                            FontSize = 30,
                TextColor = Color.FromHex("#33bbff")},
                                nativeListView }
            };

            nativeListView.ItemSelected += OnItemSelected;
        }

        private async void getPhotos()
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();

            try
            {
                Loading.Instance.showLoading();

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username } });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.PHOTOSHOOTS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                Loading.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the photo list
                        List<Photoshoot> photoshoots = jsonSystem.data.Photoshoots;

                        //Create the list view model for the listview
                        photoSesionListViewModels = new List<PhotoSesionListViewModel>();
                        PhotoSesionListViewModel photoSesionListView = new PhotoSesionListViewModel();
                        int x = 1;
                        foreach (var photo in photoshoots)
                        {
                            if (x==1)
                            {
                                photoSesionListView.photoshootLeft = photo;

                                x = 2; //Switch to right
                            }
                            else if (x==2)
                            {
                                //Last assigment and add it to the list
                                photoSesionListView.photoshootRigth = photo;                                
                                photoSesionListViewModels.Add(photoSesionListView);

                                x = 1; //Switch to left
                                photoSesionListView = new PhotoSesionListViewModel(); //New instance
                            }
                        }

                        init();
                    }
                    else
                    {
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
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
                Debug.WriteLine("Excepcion: " + ex.Message);
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
