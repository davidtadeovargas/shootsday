using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class UserLogin : ContentPage
	{
		public UserLogin()
		{
			InitializeComponent();
			touchRegister.GestureRecognizers.Add(new TapGestureRecognizer(goToRegister));
		}

		private void goToRegister(View arg1, object arg2)
		{

			Page UserRegister = new UserRegister();
			arg1.Navigation.PushModalAsync(UserRegister);

		}

		async void btnLogin(object sender, EventArgs e)
		{
			var register = (Button)sender;
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

		}

		private async void CheckUser()
		{

			/*var auxUser = new Usuario { code_event = CodeEntry.Text };
            var auxLogin = new LoginUser { password = PasswordEntry.Text, username = UserEntry.Text };*/

			try
			{
				var client = new HttpClient();
				var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { User = new { code_event = CodeEntry.Text }, Login = new { password = PasswordEntry.Text, username = UserEntry.Text } });
				//var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
				var content = new StringContent(userData, Encoding.UTF8, "application/json");

				//var uri = new Uri("http://10.0.2.57:8030/ws-jsproject/users/login.json");
				var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/users/login.json");
				//var uri = new Uri("http://localhost:8850/ws-jsproject/users/login.json");

				var result = await client.PostAsync(uri, content).ConfigureAwait(true);
				if (result.IsSuccessStatusCode)
				{
					var tokenJson = await result.Content.ReadAsStringAsync();
					var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(tokenJson);

					if (jsonSystem.status.type != "error")
					{
						// Usuario logeado correctamente
						Page masterDetail = new MasterDetail(jsonSystem);
						Navigation.PushModalAsync(masterDetail);
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
