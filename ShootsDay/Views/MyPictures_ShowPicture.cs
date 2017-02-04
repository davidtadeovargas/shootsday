using System;
using ShootsDay.Models;
using Xamarin.Forms;
using System.Diagnostics;

namespace ShootsDay
{
	public class MyPictures_ShowPicture : ContentPage
	{
        string host;
		public MyPictures_ShowPicture(Picture _picture)
		{
            this.host = Application.Current.Properties["host"].ToString();
            Uri urlImg = new Uri(this.host+""+_picture.url_image);
            try
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(15, 15),
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
                            //VerticalOptions = LayoutOptions.FillAndExpand
                        },
                    }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Excepcion: "+ex.Message);
            }
		}
	}
}

