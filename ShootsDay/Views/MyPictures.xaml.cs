using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShootsDay.Models;
using ShootsDay.Managers;
using ShootsDay.ViewModels;

namespace ShootsDay.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPictures : ContentPage
	{
		public MyPictures ()
		{
            init();	
        }

        private void init()
        {
            InitializeComponent();

            BindingContext = new MyPicturesViewModel(this);

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
            });
        }

        private void ImageTapped(object sender, TappedEventArgs e)
        {
            Picture Picture = (Picture)e.Parameter;

            RedSocialDetail RedSocialDetail = new RedSocialDetail(Picture);

            Navigation.PushModalAsync(new MasterDetail(RedSocialDetail));
        }
    }
}