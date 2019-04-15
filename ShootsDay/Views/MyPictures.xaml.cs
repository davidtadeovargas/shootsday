using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using ShootsDay.RequestModels;
using ShootsDay.Views;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class MyPictures : ContentPage
	{
		string host;
		Dictionary<string, Picture> my_pictures;
		Grid gridImg;
		public MyPictures()
		{
			InitializeComponent();
			//getMyPictures();
		}

		public async void getMyPictures()
		{
			string username = Application.Current.Properties["username"].ToString();
			string password = Application.Current.Properties["password"].ToString();
			string id_event = Application.Current.Properties["id_event"].ToString();
			this.host = Application.Current.Properties["host"].ToString();
			id_event = "7"; // Temporal.

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

				var uri = new Uri(Constants.MY_PICTURES);

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(tokenJson);

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
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(respuesta);
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
			foreach (var img in pictures)
			{
				Uri urlImg = new Uri(host + "" + img.url_image);
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
				imagen.GestureRecognizers.Add(new TapGestureRecognizer(evtClickImg));
				my_pictures.Add(img.id.ToString(), img);
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
			Debug.WriteLine("Se presiono la imagen" + arg1.ClassId);
			arg1.Navigation.PushAsync(new MyPictures_ShowPicture(my_pictures[arg1.ClassId]));
		}


        private void MainTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new RedSocial()));
        }

        private void UploadPictureTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new TakePicture()));
        }
        private void MyPicturesPictureTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new MyPictures()));
        }
    }
}
