using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

using Plugin.Media;
using System.Diagnostics;
using Plugin.Media.Abstractions;
using System.Net.Http;
using ShootsDay.Models;
using ShootsDay.RequestModels;

namespace ShootsDay.Views
{
    public class UploadPicture : ContentPage
    {
		Image img_selected;
		Button btn_upload;
        Entry description;
        private MediaFile _mediaFile;
        public UploadPicture()
        {
            //Title = "Agregar foto";
			Icon = "upload_picture_white.png";
			Button btn_upload_picture = new Button
			{
				Text = "Subír foto de la galeria"
			};
			btn_upload_picture.Clicked += Btn_Upload_Picture_Clicked;
			Button btn_take_picture = new Button
			{
				Text = "Tomar foto"
			};
			btn_take_picture.Clicked += Btn_Take_Picture_Clicked;

			btn_upload = new Button
			{
				Text = "Subír foto",
				IsEnabled = false,
				IsVisible = false
			}; 
			btn_upload.Clicked += Btn_Upload_Clicked;

			img_selected = new Image
			{
				Aspect = Aspect.AspectFill,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
            description = new Entry
            {
                Placeholder = "Descripción de la imágen",
                IsEnabled = false,
                IsVisible = false
            };
            Content = new ScrollView
			{
				Content = new StackLayout
				{
					Padding = new Thickness(20, 20),
					HorizontalOptions = LayoutOptions.Center,
					//VerticalOptions = LayoutOptions.Center,
					Children = {
						btn_upload_picture,
						btn_take_picture,
						img_selected,
                        description,
                        btn_upload
					}
				}
			};
            /*Content = new StackLayout
            {
				Padding = new Thickness(20, 20),
                HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center,
                Children = {
					btn_upload_picture,
					btn_take_picture,
					img_selected
                }
            };*/
        }

		private async void Btn_Upload_Picture_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Subír foto de la galería");
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await DisplayAlert("Error","Subír una foto no soportado","OK");
			}
            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
			if (_mediaFile == null)
				return;

			img_selected.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
			btn_upload.IsVisible = true;
			btn_upload.IsEnabled = true;

            description.IsVisible = true;
            description.IsEnabled = true;
        }

		private async void Btn_Take_Picture_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Tomar foto y subirla");

			await CrossMedia.Current.Initialize();
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("Error","Camara no displonible","Ok");
				return;
			}
            _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = "Shootsday",
				SaveToAlbum = true,
				Name = "photoshoots.jpg"
			});
			if (_mediaFile == null)
				return;
			
			img_selected.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
			btn_upload.IsVisible = true;
			btn_upload.IsEnabled = true;
            description.IsVisible = true;
            description.IsEnabled = true;
        }

		private async void Btn_Upload_Clicked(object sender, EventArgs e)
		{
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            string id_event = Application.Current.Properties["id_event"].ToString();
            try
            {
                var client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(password), "Login[password]");
                content.Add(new StringContent(username), "Login[username]");
                content.Add(new StringContent(id_event), "Picture[event_id]");
                content.Add(new StringContent(description.Text), "Picture[description]");

                if (_mediaFile != null)
                {
                    content.Add(new StreamContent(_mediaFile.GetStream()),
                    "\"image\"",
                    $"\"{_mediaFile.Path}\"");
                }
                var uri = new Uri(Constants.PICTURES_ADD);
                //var uri = new Uri("http://10.0.2.45:8030/ws-jsproject/pictures/add.json");

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);
                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);

                    if (jsonSystem.status.type != "error")
                    {
                        await DisplayAlert("Subír imágen", jsonSystem.status.message, "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert("Subír imágen", jsonSystem.status.message, "Aceptar");
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(respuesta);
                    await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }

        }
	}
}
