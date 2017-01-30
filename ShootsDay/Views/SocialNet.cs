using ImageCircle.Forms.Plugin.Abstractions;
using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ShootsDay.Views
{
    public class SocialNet : ContentPage
    {
        Dictionary<string, Picture> homePictures;
        Grid gridImg;
        string host;
        StackLayout contenedor;
        public SocialNet()
        {
            //InitializeComponent();
            //Title = "ShootsDay";
			Icon = "red_social_white.png";
            homePictures = new Dictionary<string, Picture>();
            contenedor = new StackLayout
            {
                Spacing = 20
            };
            getPicturesHome();
        }
        public async void getPicturesHome()
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            this.host = Application.Current.Properties["host"].ToString();
            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Event = new { id = id_event }, Login = new { password = password, username = username } });
                //var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
                var content = new StringContent(userData, Encoding.UTF8, "application/json");
                var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/homes.json");
                var result = await client.PostAsync(uri, content).ConfigureAwait(true);
                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestHome>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        // Se obtuvieron imagenes con éxito, se muestran en la vista
                        showPictures(jsonSystem.data.pictures);
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

        private void showPictures(List<Picture> pictures)
        {
            foreach (var img in pictures)
            {
                CircleImage imagenPerfil = new CircleImage
                {
                    WidthRequest = 60,
                    HeightRequest = 60,
                    Aspect = Aspect.AspectFill,
                    Source = (
                        (string.IsNullOrEmpty(img.User.url_image)) ?
                            "default_image.png"
                        :
                            ImageSource.FromUri(new Uri(this.host+""+img.User.url_image))
                    ),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                };

                Image imagen = new Image
                {
                    Source = ImageSource.FromUri(new Uri(img.url_image)),
                    WidthRequest = 120,
                    HeightRequest = 120,
                    Aspect = Aspect.AspectFill,
                    ClassId = img.id.ToString()
                };
                imagen.GestureRecognizers.Add(new TapGestureRecognizer(evtClickImg));
                homePictures.Add(img.id.ToString(), img);
                contenedor.Children.Add(
                    new StackLayout {
                        Padding = new Thickness(20, 20, 20, 0),
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 20,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            imagenPerfil,
                            new Label{
                                Text = img.User.username,
                            }
                        }
                    }
                );
                contenedor.Children.Add(
                new StackLayout
                    {
                        //Padding = new Thickness(20, 20),
                        HorizontalOptions = LayoutOptions.Center,
                        //Spacing = 20,
                        Children =
                        {
                            imagen
                        }
                    }
                );
            }
            this.Content = new ScrollView { Content = contenedor };
        }
        private void evtClickImg(View arg1, object arg2)
        {
            Debug.WriteLine("Se presiono la imagen" + arg1.ClassId);
            SocialNet_ShowPicture imgPage = new SocialNet_ShowPicture(homePictures[arg1.ClassId]);
            arg1.Navigation.PushAsync(imgPage);
        }
    }
}
