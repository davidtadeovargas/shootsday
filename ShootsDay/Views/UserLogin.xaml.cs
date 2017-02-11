using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;
using System.Globalization;

namespace ShootsDay
{
	public partial class UserLogin : ContentPage
	{
		public UserLogin()
		{
			InitializeComponent();
            btnLogin.Text = Recursos.AppResources.login.ToString();

            touchRegister.GestureRecognizers.Add(new TapGestureRecognizer(goToRegister));
            
            esp_lan.GestureRecognizers.Add(new TapGestureRecognizer(click_esp));
            eng_lan.GestureRecognizers.Add(new TapGestureRecognizer(click_eng));
        }

        private void AplicarIdioma()
        {
            btnLogin.Text = Recursos.AppResources.login.ToString();
            if(string.IsNullOrEmpty(UserEntry.Text))
                UserEntry.Placeholder = Recursos.AppResources.user;
            if (string.IsNullOrEmpty(PasswordEntry.Text))
                PasswordEntry.Placeholder = Recursos.AppResources.password;
            if (string.IsNullOrEmpty(CodeEntry.Text))
                CodeEntry.Placeholder = Recursos.AppResources.code_event;
            touchRegister.Text = Recursos.AppResources.btn_go_register;
        }

        private void click_eng(View arg1, object arg2)
        {
            Debug.WriteLine("La aplicación debe estar en ingles");
            //Java.Lang.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ES-US")
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");
            App.Current.Language = "eng";
            AplicarIdioma();
        }

        private void click_esp(View arg1, object arg2)
        {
            Debug.WriteLine("La aplicación debe estar en español");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("");
            App.Current.Language = "esp";
            AplicarIdioma();
        }

        private void goToRegister(View arg1, object arg2)
		{

			Page UserRegister = new UserRegister();
			arg1.Navigation.PushModalAsync(UserRegister);

		}

		public async void evt_btnLogin(object sender, EventArgs e)
		{
            var btn_login = (Button)sender;
            btn_login.IsEnabled = false;
			if (string.IsNullOrEmpty(UserEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un usuario", "Aceptar");
				UserEntry.Focus();
				return;
			}
			if (string.IsNullOrEmpty(PasswordEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un password", "Aceptar");
				PasswordEntry.Focus();
				return;
			}
			if (string.IsNullOrEmpty(CodeEntry.Text))
			{
				await DisplayAlert("Error", "Debe ingresar un código", "Aceptar");
				CodeEntry.Focus();
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
				var client = new HttpClient();
				var userData = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new {
                        Event = new { code = CodeEntry.Text },
                        Login = new { password = PasswordEntry.Text, username = UserEntry.Text, language = App.Current.Language } 
                    });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(userData, Encoding.UTF8, "application/json");

				//var uri = new Uri("http://10.0.2.57:8030/ws-jsproject/users/login.json");
				var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/users/login.json");
				//var uri = new Uri("http://192.168.0.6:8850/ws-jsproject/users/login.json");

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
                        // Usuario logeado correctamente
                        App.Current.Properties["IsLoggedIn"] = true;
                        Application.Current.Properties["user_id"] = jsonSystem.data.User.id;
                        Application.Current.Properties["username"] = jsonSystem.data.User.username;
						Application.Current.Properties["password"] = PasswordEntry.Text;
						Application.Current.Properties["id_event"] = jsonSystem.data.Event.id;
						Application.Current.Properties["host"] = jsonSystem.data.Host.url;
                        Application.Current.Properties["title_event"] = jsonSystem.data.Event.title;
                        await Application.Current.SavePropertiesAsync();
						await Navigation.PushModalAsync( new InitApp());
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
				}
			}
			catch (Exception ex)
			{
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
