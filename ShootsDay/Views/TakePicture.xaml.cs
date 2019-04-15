using Plugin.Media;
using Plugin.Media.Abstractions;
using ShootsDay.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TakePicture : ContentPage
	{
        private MediaFile _mediaFile;
        private bool hasImage = false;



        public TakePicture ()
		{
			InitializeComponent ();

            pictureEntry.GestureRecognizers.Add(new TapGestureRecognizer(uploadPicture));

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserName();
                Title = "Subir Foto";
            });
        }


        private void closeImgTapped(View arg1, object arg2)
        {
            Device.BeginInvokeOnMainThread(() => {
                profile_img.Source = "image.png";
            });

            hasImage = false;
        }

        private void uploadPicture(View arg1, object arg2)
        {
            Device.BeginInvokeOnMainThread(async () => {

                try
                {
                    Debug.WriteLine("Subír foto");

                    if (CrossMedia.Current.IsPickPhotoSupported)
                    {
                        _mediaFile = await CrossMedia.Current.PickPhotoAsync();

                        if (_mediaFile != null)
                        {
                            profile_img.Source = ImageSource.FromStream(() => _mediaFile.GetStream());

                            hasImage = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error, Excepcion: " + e.Message);
                }
            });
        }


        private async Task btnContinueTapped(object sender, TappedEventArgs e)
        {
            if (!hasImage)
            {
                await DisplayAlert("Error", "Selecciona primero una imagen", "Aceptar");                
                return;
            }
        }        

        private void MainTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new RedSocial()));
        }

        private void UploadPictureTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new TakePicture()));
        }
        private void MyPicturesPictureTapped(object sender, TappedEventArgs e)
        {
            //Picture Picture = (Picture)e.Parameter;

            Navigation.PushModalAsync(new MasterDetail(new MyPictures()));
        }
    }
}