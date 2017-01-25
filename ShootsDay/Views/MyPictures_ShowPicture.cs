using System;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public class MyPictures_ShowPicture : ContentPage
	{
		public MyPictures_ShowPicture(Picture _picture)
		{
			StackLayout stackTitle = new StackLayout
			{
				Spacing = 15,
				Padding = new Thickness(20, 20),
				HorizontalOptions = LayoutOptions.Center
			};
			Label titlePicture = new Label
			{
				Text = _picture.description,
				TextColor = Color.Black,
				FontSize = 26
			};
			stackTitle.Children.Add(titlePicture);
			StackLayout stackImg = new StackLayout
			{
				Spacing = 15,
				Padding = new Thickness(20, 20),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			Image img = new Image
			{
				Aspect = Aspect.AspectFill,
				Source = _picture.url_image
			};
			stackImg.Children.Add(img);
			Content = new StackLayout
			{
				Spacing = 15,
				Padding = new Thickness(20, 20),
				Children = {
					stackTitle,
					stackImg
				}
			};
		}
	}
}

