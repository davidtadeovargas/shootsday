﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class UserRegister : ContentPage
	{
        private MediaFile _mediaFile;
		public UserRegister()
		{
			InitializeComponent();
			UserLogin.GestureRecognizers.Add(new TapGestureRecognizer(goToLogin));
			pictureEntry.GestureRecognizers.Add(new TapGestureRecognizer(uploadPicture));
        }
		private async void uploadPicture(View arg1, object arg2)
		{
			Debug.WriteLine("Subír foto");
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Subír una foto no soportado", "OK");
            }
            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null)
                return;
            
            profile_img.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
        }
		private void goToLogin(View arg1, object arg2)
		{
			//var UserLogin = new UserLogin();
			arg1.Navigation.PopModalAsync();
			//arg1.Navigation.PushModalAsync(UserLogin);
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
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(PasswordEntry.Text), "User[password]");
                content.Add(new StringContent(UserEntry.Text), "User[username]");
                content.Add(new StringContent(NameEntry.Text), "User[name]");
                content.Add(new StringContent(EmailEntry.Text), "User[email]");
                content.Add(new StringContent(LastNameEntry.Text), "User[lastname]");
                
                if (_mediaFile != null)
                {
                    content.Add(new StreamContent(_mediaFile.GetStream()),
                    "\"image\"",
                    $"\"{_mediaFile.Path}\"");
                }
                //var uri = new Uri("http://10.0.2.149:8030/ws-jsproject/users/register.json");
                var client = new HttpClient();
                var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/users/register.json");

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);
                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(tokenJson);

                    if (jsonSystem.status.type != "error")
                    {
                        // Se registro usuario con éxito
                        await DisplayAlert("OK", jsonSystem.status.message, "Aceptar");
                        await this.Navigation.PopModalAsync();
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
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
            }
        }
    }
}
