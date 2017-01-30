using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

using Plugin.Media;
using System.Diagnostics;
using Plugin.Media.Abstractions;

namespace ShootsDay.Views
{
    public class UploadPicture : ContentPage
    {
		Image img_selected;
		Button btn_upload;
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
			var file = await CrossMedia.Current.PickPhotoAsync();
			if (file == null)
				return;

			img_selected.Source = ImageSource.FromStream(() => file.GetStream());
			btn_upload.IsVisible = true;
			btn_upload.IsEnabled = true;
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
			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = "Shootsday",
				SaveToAlbum = true,
				Name = "photoshoots.jpg"
			});
			if (file == null)
				return;
			
			img_selected.Source = ImageSource.FromStream(() => file.GetStream());
			btn_upload.IsVisible = true;
			btn_upload.IsEnabled = true;
		}

		private void Btn_Upload_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Aqui se sube la imagen");
		}
	}
}
