using ImageCircle.Forms.Plugin.Abstractions;
using ShootsDay.Managers;
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
    public class Profile : ContentPage
    {
        public Profile()
        {
            Title = "Perfil de usuario";           
            get_profile();
        }
        private void set_data_profile(Data profile)
        {
            StackLayout lista_users = new StackLayout();
            foreach (var user in profile.Event.Users)
            {
                CircleImage user_img = new CircleImage
                {
                    WidthRequest = 40,
                    HeightRequest = 40,
                    Aspect = Aspect.AspectFill,
                    Source = "default_profile.png",
                    HorizontalOptions = LayoutOptions.Center
                };
                if (!string.IsNullOrEmpty(user.url_image))
                {
                    var url = ImagesManager.Instance.geProfilePicture(user.url_image);
                    user_img.Source = ImageSource.FromUri(new Uri(url));
                }
                lista_users.Children.Add(
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(20, 20),
                        Children = {
                            user_img,
                            new Label {
                                Text = user.username,
                                HorizontalOptions = LayoutOptions.Center
                            },
                        }
                    }
                );
            }
            CircleImage profile_img = new CircleImage
            {
                WidthRequest = 120,
                HeightRequest = 120,
                Aspect = Aspect.AspectFill,
                Source = "default_profile.png",
                HorizontalOptions = LayoutOptions.Center
            };
            if (!string.IsNullOrEmpty(profile.User.url_image))
            {
                var url = ImagesManager.Instance.geProfilePicture(profile.User.url_image);
                profile_img.Source = ImageSource.FromUri(new Uri(url));
            }

            Content = new ScrollView()
            {
                
            };

            Content = new StackLayout
            {
                Padding = new Thickness(20, 20),
                Children = {
                    new Image()
                    {
                        Source="logo.png",
                        HorizontalOptions =LayoutOptions.Center,
                        Margin = 20
                    },
                    profile_img,
                    new Label {
                        Text = profile.User.name+" "+profile.User.lastname,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 28,
                    },
                    new Label {
                        Text = profile.Event.title,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 22,
                    },
                    new Label {
                        Text = profile.Event.count_users.ToString()+" contactos",
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20,
                    },
                    new ScrollView {
                        Content = lista_users
                    }
                }
            };
        }
        private async void get_profile()
        {
            Loading.Instance.showLoading();

            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            string user_id = Application.Current.Properties["user_id"].ToString();
            
            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Event = new { id = id_event },
                    Login = new { password = password, username = username }
                });
                //var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
                var content = new StringContent(userData, Encoding.UTF8, "application/json");
                var uri = new Uri(Constants.USERS_PROFILE + Convert.ToInt32(user_id)+".json");
                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                Loading.Instance.closeLoading();

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
                Loading.Instance.closeLoading();
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
            }
        }
    }
}
