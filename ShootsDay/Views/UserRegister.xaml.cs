using Plugin.Media;
using Plugin.Media.Abstractions;
using ShootsDay.Models;
using ShootsDay.RequestModels;
using ShootsDay.Models.Share;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using UIKit;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class UserRegister : ContentPage
	{
        private MediaFile _mediaFile;
        private bool hasImage = false;

        private const int NAME_FIELD_LENGTH = 44;
        private const int LASTNAME_FIELD_LENGTH = 44;
        private const int EMAIL_FIELD_LENGTH = 70;
        private const int USER_FIELD_LENGTH = 49;
        private const int PASSWORD_FIELD_LENGTH = 20;

        UserRegisterViewModel UserRegisterViewModel;




        public UserRegister()
        {
            init();
        }


        private void init()
        {
            InitializeComponent();
            pictureEntry.GestureRecognizers.Add(new TapGestureRecognizer(uploadPicture));

            UserRegisterViewModel = new UserRegisterViewModel(this);
            BindingContext = UserRegisterViewModel;

            profile_img.Source = "default_profile.png";
        }

		private void uploadPicture(View arg1, object arg2)
		{
            Device.BeginInvokeOnMainThread( async () => {

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
                catch(Exception e)
                {
                    Debug.WriteLine("Error, Excepcion: " + e.Message);
                }               
            });    
        }


        private void goToLogin(object sender, EventArgs e)
		{
            Navigation.PopModalAsync();
        }


        private void closeImgTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                profile_img.Source = "default_profile.png";
            });

            hasImage = false;
        }

        void EntryNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > NAME_FIELD_LENGTH)
            {
                NameEntry.Text = NameEntry.Text.Remove(NAME_FIELD_LENGTH);
            }
        }
        void LastNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > LASTNAME_FIELD_LENGTH)
            {
                LastNameEntry.Text = LastNameEntry.Text.Remove(LASTNAME_FIELD_LENGTH);
            }
        }
        void EmailTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > EMAIL_FIELD_LENGTH)
            {
                EmailEntry.Text = EmailEntry.Text.Remove(EMAIL_FIELD_LENGTH);
            }
        }
        void UserTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > USER_FIELD_LENGTH)
            {
                UserEntry.Text = UserEntry.Text.Remove(USER_FIELD_LENGTH);
            }
        }
        void PasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > PASSWORD_FIELD_LENGTH)
            {
                PasswordEntry.Text = PasswordEntry.Text.Remove(PASSWORD_FIELD_LENGTH);
            }
        }

        private async void btnRegister(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un usuario", "Aceptar");
                NameEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(LastNameEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un password", "Aceptar");
                LastNameEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(EmailEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un código", "Aceptar");
                EmailEntry.Focus();
                return;
            }
            if (!EmailEntry.Text.Contains("@") || !EmailEntry.Text.Contains(".")) //Validate correct email sintax
            {
                await DisplayAlert("Error", "Ingresa un correo valido", "Aceptar");
                EmailEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(UserEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un código", "Aceptar");
                UserEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un código", "Aceptar");
                PasswordEntry.Focus();
                return;
            }

            //Validate the password
            if (PasswordEntry.Text.Length<8)
            {
                await DisplayAlert("Error", "La contaseña debe de ser de por lo menos 8 caracteres", "Aceptar");
                PasswordEntry.Focus();
                return;
            }
            var containsDigit = Regex.IsMatch(PasswordEntry.Text, @"\d");
            if (!containsDigit)
            {
                await DisplayAlert("Error", "La contaseña debe de contener por lo menos un digito", "Aceptar");
                PasswordEntry.Focus();
                return;
            }
            var containsChar = Regex.IsMatch(PasswordEntry.Text, @"[a-zA-Z]");
            if (!containsChar)
            {
                await DisplayAlert("Error", "La contaseña debe de contener por lo menos un digito caracter", "Aceptar");
                PasswordEntry.Focus();
                return;
            }

            //Question before to continue
            
            var answer = await DisplayAlert("", "¿Seguro que todos los datos estan correctos?", "Si", "No");            
            if (answer)
            {
                LoadingManager.Instance.showLoading();

                try
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    content.Add(new StringContent(PasswordEntry.Text), "User[password]");
                    content.Add(new StringContent(UserEntry.Text), "User[username]");
                    content.Add(new StringContent(NameEntry.Text), "User[name]");
                    content.Add(new StringContent(EmailEntry.Text), "User[email]");
                    content.Add(new StringContent(LastNameEntry.Text), "User[lastname]");

                    if (hasImage)
                    {
                        content.Add(new StreamContent(_mediaFile.GetStream()),
                        "\"image\"",
                        $"\"{_mediaFile.Path}\"");
                    }
                    var client = new HttpClient();
                    var uri = new Uri(Constants.USERS_REGISTER);

                    var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                    LoadingManager.Instance.closeLoading();

                    if (result.IsSuccessStatusCode)
                    {
                        var tokenJson = await result.Content.ReadAsStringAsync();
                        var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(tokenJson);

                        if (jsonSystem.status.type != "error")
                        {
                            // Se registro usuario con éxito
                            await DisplayAlert("", jsonSystem.status.message, "Aceptar");
                            await this.Navigation.PopModalAsync();

                            App.Current.MainPage = new UserLogin(UserEntry.Text, PasswordEntry.Text);                            
                        }
                        else
                        {
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
