using ShootsDay.Managers;
using ShootsDay.Models;
using ShootsDay.Models.Share;
using ShootsDay.RequestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedSocialDetail : ContentPage
    {
        Picture Picture_;
        RedSocialDetailViewModel RedSocialDetailViewModel;

        


        public RedSocialDetail(Picture Picture)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            Picture_ = Picture; //Save current photo

            /*
                Init the view model
             */
            RedSocialDetailViewModel = new RedSocialDetailViewModel(this, Picture_, lblNoItemss, listListview);            
            BindingContext = RedSocialDetailViewModel; //Attach the binding context

            /*
                Load the title and image
             */
            Device.BeginInvokeOnMainThread(() =>
            {
                string url = Picture_.url_image;
                image.Source = ImageSource.FromUri(new Uri(url));

                //userType.Source = ImageSource.FromUri(new Uri(url));

                RedSocialDetailViewModel.Source = image.Source;//For binding

                Title = "Red Social";

                lbCounterLikes.Text = Picture_.likes.ToString();
                lbCounterComments.Text = Picture_.comments.ToString();
                //lbUserTitle.Text = Picture_.User.username;
                lbTitle.Text = Picture_.description;
                lbDatePicture.Text = Picture_.created_format;

                lblUserName.Text = Picture_.User.username;
                url = Picture_.User.url_image;
                imgUserPicture.Source = ImageSource.FromUri(new Uri(url));
            });
        }

        
        private void CommentTextChange(object sender, EventArgs e)
        {
            string comment = messageEntry.Text==null?"": messageEntry.Text.ToString().Trim();
            if (comment == "")
            {
                btnEnviar.IsEnabled = false;
            }
            else
            {
                btnEnviar.IsEnabled = true;
            }
        }


        private void ViewProfileTapped(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;

            Profile_ Profile_ = new Profile_(Picture_.User.id);

            Navigation.PushModalAsync(new MasterDetail(Profile_));
        }

        private async void OnSendClickedAsync(object sender, EventArgs e)
        {
            btnEnviar.IsEnabled = false;

            //Send the comment to the server
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            string comment = messageEntry.Text;

            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Picture = new { comment = comment, picture_id = Picture_ .id}, Login = new { password = password, username = username }});
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.ADD_COMMENT);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                btnEnviar.IsEnabled = true;

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestAddComment>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Reload the comments
                        RedSocialDetailViewModel.comments.Clear();
                        RedSocialDetailViewModel.getComments();
                    }
                    else
                    {
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");                        
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(respuesta);
                    await DisplayAlert("Error", respuesta, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                btnEnviar.IsEnabled = true;
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }

        }

        private async Task OnItemClicked(object sender, EventArgs e)
        {
            Picture Picture = (Picture)sender;

                
        }
    }
}