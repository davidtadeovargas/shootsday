using System;
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

/*
    https://github.com/aritchie/userdialogs
     */
namespace ShootsDay
{
	public partial class UserLogin : ContentPage
    {
        Button btn_login = null;


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

            //Close session
            SettingsManager.Instance.setIsNotLoggedIn().SavePropertiesAsync();            

            btnLogin.Text = Recursos.AppResources.login.ToString();

            link.FontSize = 40;
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

        private void OnRegistrarClicked(object sender, EventArgs e)
        {
			Page UserRegister = new UserRegister();
            Navigation.PushModalAsync(UserRegister);            

		}

		public async void evt_btnLogin(object sender, EventArgs e)
		{
            btn_login = (Button)sender;
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
                LoadingManager.Instance.showLoading();

                var client = new HttpClient();
				var userData = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new {
                        Event = new { code = CodeEntry.Text },
                        Login = new { password = PasswordEntry.Text, username = UserEntry.Text, language = App.Current.Language } 
                    });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(userData, Encoding.UTF8, "application/json");

				//var uri = new Uri("http://10.0.2.57:8030/ws-jsproject/users/login.json");
				var uri = new Uri(Constants.USERS_LOGIN);
				//var uri = new Uri("http://192.168.0.6:8850/ws-jsproject/users/login.json");

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
                        settingsManager.setPassword(PasswordEntry.Text);
                        settingsManager.setIdEvent(jsonSystem.data.Event.id);
                        settingsManager.setHost(jsonSystem.data.Host.url);
                        settingsManager.setRoleId(jsonSystem.data.User.role_id);
                        settingsManager.setIsSuperUser(jsonSystem.data.User.super);
                        settingsManager.setTitleEvent(jsonSystem.data.Event.title);
                        await settingsManager.SavePropertiesAsync();
                        
                        await Navigation.PushModalAsync(new MasterDetail(new Home_()));
					}
					else
					{
                        btn_login.IsEnabled = true;
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
