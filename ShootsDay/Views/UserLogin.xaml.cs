using System;
using System.Collections.Generic;

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

			Page mainPage = new MainPage();
			Navigation.PushModalAsync(mainPage);
		}
	}
}
