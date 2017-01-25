using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class Photoshoots : CarouselPage
	{
		string host;
		public Photoshoots()
		{
			//InitializeComponent();
			Title = "Photoshoots";
			var padding = new Thickness(0, Device.OnPlatform(40, 40, 0), 0, 0);
			var redContentPage = new ContentPage
			{
				Padding = padding,
				Content = new StackLayout
				{
					Children = {
						new Label {
							Text = "Red",
							FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
							HorizontalOptions = LayoutOptions.Center
						},
						new BoxView {
							Color = Color.Red,
							WidthRequest = 200,
							HeightRequest = 200,
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.CenterAndExpand
						}
					}
				}
			};
			var greenContentPage = new ContentPage
			{
				Padding = padding,
				Content = new StackLayout
				{
					
					Children = {
						new Label {
							Text = "green",
							FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
							HorizontalOptions = LayoutOptions.Center
						},
						new BoxView {
							Color = Color.Green,
							WidthRequest = 200,
							HeightRequest = 200,
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.CenterAndExpand
						}
					}
            	}
			};
			getPhotos();
		}

		private void set_pictures(List<Photoshoot> pictures)
		{ 
			foreach (var picture in pictures)
			{
				Uri urlImg = new Uri(picture.url_image);
				var newPicture = new ContentPage
				{
					//Padding = padding,
					Content = new StackLayout
					{
						Spacing = 20,
						Padding = new Thickness(20, 20),
						//VerticalOptions = LayoutOptions.CenterAndExpand,

						Children = {
							new Label {
									Text = picture.description,
								FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
								HorizontalOptions = LayoutOptions.Center
							},
							new Image{
								Source = urlImg,
								Aspect = Aspect.AspectFill
							}
						}
					}
				};
				Children.Add(newPicture);
			}
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

				var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/photoshoots.json");

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
						set_pictures(jsonSystem.data.Photoshoots);
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
				await DisplayAlert("", ex.Message, "Aceptar");
			}
		}
	}
}
