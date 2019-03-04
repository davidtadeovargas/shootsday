using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;
using ShootsDay.Views;

namespace ShootsDay
{
	public class SocialNet_ShowPicture : ContentPage
	{
        ShareImageViewModel _shareImageViewModel;
        string username = Application.Current.Properties["username"].ToString();
		string password = Application.Current.Properties["password"].ToString();
		string id_event = Application.Current.Properties["id_event"].ToString();
		string host = Application.Current.Properties["host"].ToString();

		Image imgLikes;
		Label countLikes;
		Picture picture;
		public SocialNet_ShowPicture(Picture _picture)
		{
			this.picture = _picture;
            Uri urlImg = new Uri(_picture.url_image);
            _shareImageViewModel = new ShareImageViewModel();
            BindingContext = _shareImageViewModel;

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
            Content = new StackLayout
            {
                Padding = new Thickness(15, 15),
                    //VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    new Label {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = _picture.description,
                        TextColor = Color.Black,
                        FontSize = 26,
                    },
                    new Image{
                        Source = urlImg,
                        Aspect = Aspect.AspectFill,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    },
                    new StackLayout {
                        Spacing = 15,
                        Padding = new Thickness(20, 20),
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {
                            imgComments,
                            imgLikes,
                            countLikes
                        }
                    }
                },
            };
            var contentPage = new ContentPage();
            this.ToolbarItems.Add(new ToolbarItem
            {
                Text="Shared",
            });
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

				var uri = new Uri(Constants.LIKES_ADD);

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

				var uri = new Uri(Constants.COMMENTS_GETLIST);

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestComments>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
                        // Mostrar los comentarios de la imagen
                        await this.Navigation.PushAsync(new SocialNet_ShowComments(jsonSystem, arg1.ClassId));
					}
					else
					{
						//await DisplayAlert("Alerta", jsonSystem.status.message, "Aceptar");
                        await this.Navigation.PushAsync(new SocialNet_ShowComments(jsonSystem, arg1.ClassId));
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

