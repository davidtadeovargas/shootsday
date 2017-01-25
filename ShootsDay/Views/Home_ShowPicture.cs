using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public class Home_ShowPicture : ContentPage
	{
		string username = Application.Current.Properties["username"].ToString();
		string password = Application.Current.Properties["password"].ToString();
		string id_event = Application.Current.Properties["id_event"].ToString();
		string host = Application.Current.Properties["host"].ToString();

		Image imgLikes;
		Label countLikes;
		Picture picture;
		public Home_ShowPicture(Picture _picture)
		{
			this.picture = _picture;
			StackLayout stackElements = new StackLayout
			{ 
				//Spacing = 15,
				Padding = new Thickness(15,15),
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			Label titlePicture = new Label
			{ 
				Text = _picture.description,
				TextColor = Color.Black,
				FontSize = 26,
				HorizontalOptions = LayoutOptions.Center
			};


			/*StackLayout stackImg = new StackLayout
			{ 
				Spacing = 15,
				Padding = new Thickness(20, 20),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};*/
			Image img = new Image
			{
				Aspect = Aspect.AspectFill,
				Source = _picture.url_image,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			stackElements.Children.Add(titlePicture);
			stackElements.Children.Add(img);
			StackLayout stackButtons = new StackLayout
			{
				Spacing = 15,
				Padding = new Thickness(20, 20),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center
				//BackgroundColor = Color.Lime
			};
			Image imgComments = new Image
			{
				Source = "comment.png",
				Aspect = Aspect.AspectFill,
				ClassId = _picture.id.ToString()
			};
			imgComments.GestureRecognizers.Add(new TapGestureRecognizer(get_comments));
			imgLikes = new Image
			{
				Source = "like.png",
				Aspect = Aspect.AspectFill
			};
			if (!_picture.like_user)
			{
				imgLikes.Source = "like_black.png";
				imgLikes.GestureRecognizers.Add(new TapGestureRecognizer(evtClickLike));
			}
			countLikes = new Label
			{
				Text = _picture.likes.ToString(),
				TextColor = Color.FromHex("#07cd92")
			};
			stackButtons.Children.Add(imgComments);
			stackButtons.Children.Add(imgLikes);
			stackButtons.Children.Add(countLikes);

			Content = new StackLayout
			{
				Spacing = 15,
				Padding = new Thickness(20, 20),
				Children = {
					stackElements,
					//stackImg,
					stackButtons
				}
			};
		}

		private async void evtClickLike(View arg1, object arg2)
		{
			// Se manda el like al WS y se cambia la imagen de color
			Debug.WriteLine("Se da like en la imagen con ID: "+this.picture.id);

			try
			{
				var client = new HttpClient();
				var data = Newtonsoft.Json.JsonConvert.SerializeObject(new { Like = new { picture_id = this.picture.id }, Login = new { password = password, username = username } });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(data, Encoding.UTF8, "application/json");

				var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/likes/add.json");

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestAddLike>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
						// Se registra el like con éxito
						imgLikes.Source = "like.png";
						countLikes.Text = (this.picture.likes + 1).ToString();
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

		private async void get_comments(View arg1, object arg2)
		{
			// Traer los comentarios de la imagen y mostrarlos en una nueva página.
			Debug.WriteLine("ID de la imagen: "+this.picture.id);

			try
			{
				var client = new HttpClient();
				var data = Newtonsoft.Json.JsonConvert.SerializeObject(new { Picture = new { id = this.picture.id }, Login = new { password = password, username = username } });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(data, Encoding.UTF8, "application/json");

				var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/comments/get_list_comments.json");

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestComments>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
						// Mostrar los comentarios de la imagen
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
	}
}

