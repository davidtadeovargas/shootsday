using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ShootsDay
{
	public class InitApp : ContentPage
	{
		public delegate void btnSocialNet();
		public event btnSocialNet evtBtnSocialNet; 

		public delegate void btnInvitation();
		public event btnInvitation evtBtnInvitation;

		public delegate void btnSession();
		public event btnSession evtBtnSession;
		public InitApp()
		{
			Button btnRedSocial = new Button
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

		void BtnRedSocial_Clicked(object sender, EventArgs e)
		{
			if (evtBtnSocialNet != null)
				evtBtnSocialNet();
		}

		private void BtnInvitacion_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Abrir ventana de invitación");
			if (evtBtnInvitation != null)
				evtBtnInvitation();
		}

		private void BtnSesion_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Abrir ventana de Sesion de fotos");
			if (evtBtnSession != null)
				evtBtnSession();
		}
	}
}


