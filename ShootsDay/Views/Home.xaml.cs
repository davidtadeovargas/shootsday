using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class Home : ContentPage
	{
		Dictionary<string, Picture> homePictures;
		Grid gridImg;
		string host;
		public Home()
		{
			//InitializeComponent();
			Title = "ShootsDay";
			StackLayout stack = new StackLayout()
			{ 
				Spacing = 15,
				Padding = new Thickness(20,20)
			};
			gridImg = new Grid()
			{ 
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			stack.Children.Add(gridImg);
			this.Content = new ScrollView { Content = stack };
			homePictures = new Dictionary<string, Picture>();
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
			
			gridImg.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			gridImg.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			bool flag = true;
			int numRow = 0;
			int borrar = 0;
			foreach (var img in pictures)
			{
				Uri urlImg = new Uri(img.url_image);
				if (borrar == 3)
				{ 
					urlImg = new Uri(host + "" + img.url_image);
				}
				borrar++;
				Image imagen = new Image
				{
					Source = urlImg,
					MinimumWidthRequest = 50,
					WidthRequest = 50,
					MinimumHeightRequest = 50,
					HeightRequest = 50,
					Aspect = Aspect.AspectFill,
					ClassId = img.id.ToString()
				};
				imagen.GestureRecognizers.Add( new TapGestureRecognizer(evtClickImg));
				homePictures.Add(img.id.ToString(), img);
				if (flag)
				{
					gridImg.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
					gridImg.Children.Add(imagen, 0, numRow);
					flag = false;
				}
				else
				{ 
					gridImg.Children.Add(imagen, 1, numRow);
					numRow++;
					flag = true;
				}
			}
		}

		void evtClickImg(View arg1, object arg2)
		{
			Debug.WriteLine("Se presiono la imagen"+arg1.ClassId);
			Home_ShowPicture imgPage = new Home_ShowPicture(homePictures[arg1.ClassId]);
			arg1.Navigation.PushAsync(imgPage);
		}
}
}
