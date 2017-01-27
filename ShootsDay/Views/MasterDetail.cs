using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using Xamarin.Forms;

namespace ShootsDay
{
	public class MasterDetail : MasterDetailPage
	{
		public User usuario;
		MenuPage menuPage;
		public MasterDetail(Page page)
		{
			menuPage = new MenuPage();
			menuPage.evtItemSelected += MenuPage_EvtItemSelected;
			Master = menuPage;
			/*Home inicio = new Home(user.data.user);
			Detail = new NavigationPage(inicio);*/
			InitApp init = new InitApp();
            //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(page)));
            Detail = new NavigationPage(page);
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