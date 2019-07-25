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

            BindingContext = new MyPicturesViewModel(this,list,noRecords);

            Device.BeginInvokeOnMainThread(() => {
                //NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
                imgProfile.Source = SettingsManager.Instance.getuSerUrlImage();
            });//UserEvents
        }

        private void OnMisEventosTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MasterDetail(new UserEvents()));
        }
        
        private void ImageTapped(object sender, EventArgs e)
        {
            Picture Picture = (Picture)e;

            RedSocialDetail RedSocialDetail = new RedSocialDetail(Picture);

            Navigation.PushAsync(new MasterDetail(RedSocialDetail));            
        }
    }
}