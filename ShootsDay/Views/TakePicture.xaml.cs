using Plugin.Media;
using Plugin.Media.Abstractions;
using ShootsDay.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ShootsDay.RequestModels;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShootsDay.Models.Share;

namespace ShootsDay.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TakePicture : ContentPage
	{
        private MediaFile _mediaFile;
        private bool hasImage = false;



        public TakePicture ()
		{
            NavigationPage.SetHasNavigationBar(this,false);

            InitializeComponent ();

            BindingContext = new TakePictureViewModel(this);

            pictureEntry.GestureRecognizers.Add(new TapGestureRecognizer(uploadPicture));

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";                
            });
        }


        private void closeImgTapped(object sender, EventArgs e)
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


        private async void OnSubirTapped(object sender, EventArgs e)
        {
            //Can not continue without image
            if (!hasImage)
            {
                await DisplayAlert("Error", "Selecciona primero una imagen", "Aceptar");                
                return;
            }

            //Can not continue without comment
            var comment = editorComment.Text==null?"": editorComment.Text.Trim();
            if (comment == "")
            {
                await DisplayAlert("Error", "Ingresa un comentario", "Aceptar");
                return;
            }

            /*
                Question before to continue
             */
            var answer = await DisplayAlert("", "¿Continuar?", "Si", "No");
            if (answer)
            {
                LoadingManager.Instance.showLoading();

                string user = SettingsManager.Instance.getUserName();
                string password = SettingsManager.Instance.getPassword();
                string idEvent = SettingsManager.Instance.getIdEvent().ToString();

                try
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    content.Add(new StringContent(password), "Login[password]");
                    content.Add(new StringContent(user), "Login[username]");
                    content.Add(new StringContent(idEvent), "Picture[event_id]");
                    content.Add(new StringContent(comment), "Picture[comment]");                    

                    if (hasImage)
                    {
                        content.Add(new StreamContent(_mediaFile.GetStream()),
                        "\"image\"",
                        $"\"{_mediaFile.Path}\"");
                    }
                    var client = new HttpClient();
                    var uri = new Uri(Constants.UPLOAD_PICTURE);

                    var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                    LoadingManager.Instance.closeLoading();

                    if (result.IsSuccessStatusCode)
                    {
                        var tokenJson = await result.Content.ReadAsStringAsync();
                        var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(tokenJson);

                        if (jsonSystem.status.type != "error")
                        {
                            // Photo uploaded succesfully
                            await DisplayAlert("", jsonSystem.status.message, "Aceptar");
                            await this.Navigation.PopModalAsync();                            
                        }
                        else
                        {
                            // Error uploading photo
                            await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                        }
                    }
                    else
                    {
                        var respuesta = await result.Content.ReadAsStringAsync();
                        var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(respuesta);
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    LoadingManager.Instance.closeLoading();
                    Debug.WriteLine("Error, Excepcion: " + ex.Message);
                }
            }
        }
    }
}