using System;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public class MyPictures_ShowPicture : ContentPage
	{
		public MyPictures_ShowPicture(Picture _picture)
		{
            Uri urlImg = new Uri(_picture.url_image);
            
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
                        VerticalOptions = LayoutOptions.FillAndExpand
                    },
                }
                /*Children = {
					stackTitle,
					stackImg
				}*/
            };
		}
	}
}

