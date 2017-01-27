using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ShootsDay
{
	public class InitApp : ContentPage
	{
        Button btnRedSocial;

        public InitApp()
		{
			btnRedSocial = new Button
			{
				Text = "Red social",
				TextColor = Color.FromHex("#01cb8f"), 
				BorderColor = Color.Gray,
				BorderWidth = 2,
			};
			Button btnInvitacion = new Button 
			{ 
				Text = "Invitación",
				TextColor = Color.FromHex("#01cb8f"),
				BorderColor = Color.Gray,
				BorderWidth = 2,
			};
			Button btnSesion = new Button { 
				Text = "Sesión de fotos",
				TextColor = Color.FromHex("#01cb8f"),
				BorderColor = Color.Gray,
				BorderWidth = 2,
			};


			Content = new StackLayout
			{
				Padding = new Thickness(20, 20),
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.End,
				Children = {
					btnRedSocial,
					btnInvitacion,
					btnSesion
				}
			};
			btnRedSocial.Clicked += BtnRedSocial_Clicked;
			btnInvitacion.Clicked += BtnInvitacion_Clicked;
			btnSesion.Clicked += BtnSesion_Clicked;
		}
		public void BtnRedSocial_Clicked(object sender, EventArgs e)
		{
            Navigation.PushModalAsync(new MasterDetail(new Home()));
        }

		private void BtnInvitacion_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Abrir ventana de invitación");
            Navigation.PushModalAsync(new MasterDetail(new Invites()));
        }

		private void BtnSesion_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Abrir ventana de Sesion de fotos");
            Navigation.PushModalAsync(new MasterDetail(new Photoshoots()));
        }
	}
}


