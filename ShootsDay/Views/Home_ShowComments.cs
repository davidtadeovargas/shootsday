using System;

using Xamarin.Forms;

namespace ShootsDay
{
	public class Home_ShowComments : ContentPage
	{
		public Home_ShowComments()
		{
			ListView listComments = new ListView
			{ 
			};

			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}

