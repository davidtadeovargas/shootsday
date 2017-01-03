using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class UserRegister : ContentPage
	{
		public UserRegister()
		{
			InitializeComponent();
			UserLogin.GestureRecognizers.Add(new TapGestureRecognizer(goToLogin));
			pictureEntry.GestureRecognizers.Add(new TapGestureRecognizer(uploadPicture));
		}
		private void uploadPicture(View arg1, object arg2)
		{
			Debug.WriteLine("Subír foto");
		}

		private void goToLogin(View arg1, object arg2)
		{
			//var UserLogin = new UserLogin();
			arg1.Navigation.PopModalAsync();
			//arg1.Navigation.PushModalAsync(UserLogin);
		}

		void btnRegister(object sender, EventArgs e)
		{
			var b = (Button)sender;
			DisplayAlert("Registrar", "Verificar los datos con el web services", "Prueba");
		}
	}
}
