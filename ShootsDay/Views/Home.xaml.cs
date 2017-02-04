using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;
using ShootsDay.Views;

namespace ShootsDay
{
	public partial class Home : TabbedPage
    {
		public Home()
		{
            this.Title = "ShootsDay";
			BarBackgroundColor = Color.FromHex("#01cb8f");

            this.Children.Add(new SocialNet());

            this.Children.Add(new UploadPicture());

            this.Children.Add(new MyPictures());
        }
    }
}
