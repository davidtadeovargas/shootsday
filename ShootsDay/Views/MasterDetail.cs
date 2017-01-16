using System;
using System.Diagnostics;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public class MasterDetail : MasterDetailPage
	{
		MenuPage menuPage;
		public MasterDetail(RequestUser user)
		{
			Debug.WriteLine("Usuario: "+user.data.user.username);
			Debug.WriteLine("Email de Usuario: " + user.data.user.email);
			Debug.WriteLine("Imagen de perfil de usuario: " + user.data.user.url_image);
			menuPage = new MenuPage();
			menuPage.evtItemSelected += MenuPage_EvtItemSelected;
			Master = menuPage;
			Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Home)));
		}

		void MenuPage_EvtItemSelected(object item)
		{
			var itemSelect = (MasterPageItem)item;
			Type page = itemSelect.TargetType;

			Detail = new NavigationPage((Page)Activator.CreateInstance(page));
			IsPresented = false;
		}
	}
}

