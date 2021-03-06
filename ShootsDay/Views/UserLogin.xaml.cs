﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;
using System.Globalization;
using Acr.UserDialogs;
using System.Threading.Tasks;
using ShootsDay.Views;
using ShootsDay.Managers;
using ShootsDay.RequestModels;
using ShootsDay.ViewModels;

/*
    https://github.com/aritchie/userdialogs
     */
namespace ShootsDay
{
	public partial class UserLogin : ContentPage
    {
        private bool _userTapped;

        private const int LOGIN_FIELD_LENGTH = 70;
        private const int PASSWORD_FIELD_LENGTH = 20;
        private const int EVENT_FIELD_LENGTH = 10;

        public UserLogin()
		{
            Init();

            /*
                For testing
             */
            Device.BeginInvokeOnMainThread(() => {
                UserEntry.Text = "master";
                PasswordEntry.Text = "123456789";
                CodeEntry.Text = "50019";
            });
        }


        public UserLogin(string user, string password)
        {
            Init();

            
            /*
                Set the user and password
             */
            Device.BeginInvokeOnMainThread(() => {
                UserEntry.Text = user;
                PasswordEntry.Text = password;
            });
        }

        private void Init()
        {

            InitializeComponent();

            var LoginViewModel_ = new LoginViewModel(this);
            BindingContext = LoginViewModel_;

            //Close session
            SettingsManager.Instance.setIsNotLoggedIn().SavePropertiesAsync();
        }


        void EntryLoginTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > LOGIN_FIELD_LENGTH)
            {
                UserEntry.Text = UserEntry.Text.Remove(LOGIN_FIELD_LENGTH);
            }
        }

        void EntryPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > PASSWORD_FIELD_LENGTH)
            {
                PasswordEntry.Text = PasswordEntry.Text.Remove(PASSWORD_FIELD_LENGTH);
            }
        }

        void EntryCodeEventTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > EVENT_FIELD_LENGTH)
            {
                CodeEntry.Text = CodeEntry.Text.Remove(EVENT_FIELD_LENGTH);
            }
        }

        private void AplicarIdioma()
        {
            if(string.IsNullOrEmpty(UserEntry.Text))
                UserEntry.Placeholder = Recursos.AppResources.user;
            if (string.IsNullOrEmpty(PasswordEntry.Text))
                PasswordEntry.Placeholder = Recursos.AppResources.password;
            if (string.IsNullOrEmpty(CodeEntry.Text))
                CodeEntry.Placeholder = Recursos.AppResources.code_event;            
        }

        private void click_eng(View arg1, object arg2)
        {
            Debug.WriteLine("La aplicación debe estar en ingles");
            //Java.Lang.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ES-US")
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");
            //App.Current.Language = "eng";
            AplicarIdioma();
        }

        private void click_esp(View arg1, object arg2)
        {
            Debug.WriteLine("La aplicación debe estar en español");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("");
            //App.Current.Language = "esp";
            AplicarIdioma();
        }

        private async void OnRegistrarClicked(object sender, EventArgs e)
        {
            if (_userTapped)
                return;

            _userTapped = true;

            Page UserRegister = new UserRegister();
            await Navigation.PushModalAsync(UserRegister);
            _userTapped = false;
        }

        public async void evt_btnLogin(object sender, EventArgs e)
		{
            if (_userTapped)
                return;

            _userTapped = true;
            
            btn_login.IsEnabled = false;
			if (string.IsNullOrEmpty(UserEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un usuario", "Aceptar");
				UserEntry.Focus();
                _userTapped = false;
                return;
			}
			if (string.IsNullOrEmpty(PasswordEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un password", "Aceptar");
				PasswordEntry.Focus();
                _userTapped = false;
                return;
			}
			if (string.IsNullOrEmpty(CodeEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un código", "Aceptar");
				CodeEntry.Focus();
                _userTapped = false;
                return;
			}
			// Se hace la conexion al Web services de Usuarios
			CheckUser();
            //btn_login.IsEnabled = true;
        }

		private async void CheckUser()
		{
			try
			{
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
				var userData = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new {
                        Event = new { code = CodeEntry.Text },
                        Login = new { password = PasswordEntry.Text, username = UserEntry.Text, language = "esp" } 
                    });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(userData, Encoding.UTF8, "application/json");

			    var uri = new Uri(Constants.USERS_LOGIN);
				
				var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
                        SettingsManager settingsManager = SettingsManager.Instance;

                        // Usuario logeado correctamente
                        settingsManager.setIsLoggedIn();
                        settingsManager.setUserId(jsonSystem.data.User.id);
                        settingsManager.setUserName(jsonSystem.data.User.username);
                        settingsManager.setTUserUrlImage(jsonSystem.data.User.url_image);
                        settingsManager.setUserLargeName(jsonSystem.data.User.fullname);
                        settingsManager.setPassword(PasswordEntry.Text);
                        settingsManager.setIdEvent(jsonSystem.data.Event.id);
                        settingsManager.setEventCode(jsonSystem.data.Event.code);
                        settingsManager.setHost(jsonSystem.data.Host.url);
                        settingsManager.setRoleId(jsonSystem.data.User.role_id);
                        settingsManager.setIsSuperUser(jsonSystem.data.User.super);
                        settingsManager.setTitleEvent(jsonSystem.data.Event.title);
                        await settingsManager.SavePropertiesAsync();                        

                        await Navigation.PushModalAsync(new MasterDetail(new Home_()));
                        _userTapped = false;
                    }
					else
					{
                        await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");

                        _userTapped = false;
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
                LoadingManager.Instance.closeLoading();
                btn_login.IsEnabled = true;
                Debug.WriteLine("Excepcion: " + ex.Message);
				await DisplayAlert("", ex.Message, "Aceptar");
			}


			/*var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.js-project.com.mx");

            string jsonData = @"{""username"" : ""CodeEntry.Text"",""code_event"" : ""3030"", ""password"" : ""mypassword""}";
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { data = new { User = auxUser, Login = auxLogin } }));

            //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/ws-jsproject/users/login", content);

            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            var result = await response.Content.ReadAsStringAsync();
            var jsonR = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(response.ToString());
            Debug.WriteLine("Respuesta");*/

			/*string url = "http://10.0.2.57:8030/ws-jsproject/users/login.json?code_event=" + CodeEntry.Text+ "&username=" + UserEntry.Text+ "&password=" + PasswordEntry.Text;
            //string url = "http://www.js-project.com.mx/ws-jsproject/users/";
            var list = await url.GetJson<List<User>>();
            if (default(List<User>) != list)
            {
                if (list[0].status == "OK")
                {
                    Debug.WriteLine("Resultado");
                }
            }*/
		}
	}
}
