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
			
			//Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(page)));
			Detail = new NavigationPage(page) 
			{
				BarBackgroundColor = Color.FromHex("#01cb8f"),
				BarTextColor = Color.White
			};
		}

		void MenuPage_EvtItemSelected(object item)
		{
			var itemSelect = (MasterPageItem)item;
			Type page = itemSelect.TargetType;
            IsPresented = false;
            if (page.Name == "UserLogin")
            {
                // Se sale de la aplicacion y se borran las propiedades
                if (Application.Current.Properties.ContainsKey("username") || Application.Current.Properties.ContainsKey("password"))
                {
                    Application.Current.Properties.Remove("username");
                    Application.Current.Properties.Remove("password");
                    Application.Current.Properties.Remove("id_event");
                    Application.Current.Properties.Remove("host");
                }
                //this.Navigation.PopToRootAsync();
                App.Current.Logout();
            }
            else
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(page))
                {
                    BarBackgroundColor = Color.FromHex("#01cb8f"),
                    BarTextColor = Color.White
                };
            }
        }
	}
}