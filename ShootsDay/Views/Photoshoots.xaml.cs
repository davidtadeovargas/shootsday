using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
    public partial class Photoshoots : ContentPage
    {
        ShareImageViewModel _shareImageViewModel;
        private int _currentImageId = 0;
        string host;
        Image imagen_active;
        List<string> list_images;
        public Photoshoots()
        {
            Title = "Photoshoots";
            list_images = new List<string>();
            InitializeComponent();
            getPhotos();
            _shareImageViewModel = new ShareImageViewModel();
            
            BindingContext = _shareImageViewModel;
            btnPrevius.IsEnabled = false;
            btnNext.IsEnabled = false;
        }
        

        private void set_pictures(List<Photoshoot> pictures)
        {
            foreach (var picture in pictures)
            {
                Uri urlImg = new Uri(picture.url_image);
                var newLabel = new ContentPage
                {
                    Content = new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = picture.description,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    }
                };
                imagen_active = new Image
                {
                    Source = urlImg,
                    Aspect = Aspect.AspectFill,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Button btn_downloadImage = new Button
                {
                    Text = "download",
                };
                btn_downloadImage.Clicked += Btn_downloadImage_Clicked;
                var newPicture = new ContentPage
                {
                    Padding = new Thickness(15, 15),
                    Title = picture.description,
                    Content = new StackLayout
                    {
                        //VerticalOptions = LayoutOptions.FillAndExpand,
                        Children = {
                            new Label {
                                HorizontalOptions = LayoutOptions.Center,
                                Text = picture.description,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                            },
                            imagen_active
                        }
                    }
                };
                //Children.Add(newPicture);
            }
        }

        private void Btn_downloadImage_Clicked(object sender, EventArgs e)
        {
        }
        void loadImages(List<Photoshoot> images)
        {
            foreach (var img in images)
            {
                list_images.Add(img.url_image);
            }
        }
        void LoadImage()
        {
            image_photoshoot.Source = new UriImageSource
            {
                Uri = new Uri(list_images[_currentImageId]),
                //CachingEnabled = false
            };
            _shareImageViewModel.Source = image_photoshoot.Source;

        }
        void Previous_Clicked(object sender, System.EventArgs e)
        {
            
            if (_currentImageId == 0)
                _currentImageId = list_images.Count - 1;
            else
                _currentImageId--;

            LoadImage();
        }

        void Next_Clicked(object sender, System.EventArgs e)
        {
            if (_currentImageId == list_images.Count - 1)
                _currentImageId = 0;
            else
                _currentImageId++;

            LoadImage();
        }

        private async void getPhotos()
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            this.host = Application.Current.Properties["host"].ToString();
            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username } });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.PHOTOSHOOTS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);
                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //set_pictures(jsonSystem.data.Photoshoots);
                        loadImages(jsonSystem.data.Photoshoots);
                        LoadImage();
                        btnPrevius.IsEnabled = true;
                        btnNext.IsEnabled = true;
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
