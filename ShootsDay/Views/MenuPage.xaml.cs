using ShootsDay.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ShootsDay
{
	public partial class MenuPage : ContentPage
	{
		public delegate void ItemSelected(object item);
		public event ItemSelected evtItemSelected;
		public List<MasterPageItem> menuList { get; set; }
		public MenuPage()
		{
			Title = "Menu";
			Icon = "menu-collapse.png";
			InitializeComponent();
			menuList = new List<MasterPageItem>();

			// Creating our pages for menu navigation
			// Here you can define title for item, 
			// icon on the left side, and page that you want to open after selection
			var init_page = new MasterPageItem() { Title = "Inicio", Icon = "home.png", TargetType = typeof(Home_) };
			var contact_page = new MasterPageItem() { Title = "Contacto", Icon = "contact.png", TargetType = typeof(Contact) };
            var miPerfil_page = new MasterPageItem() { Title = "Mi perfil", Icon = "profile.png", TargetType = typeof(Profile) };
            var salir_page = new MasterPageItem() { Title = "Cerrar sesión", Icon = "logout.png", TargetType = typeof(UserLogin) };

            // Adding menu items to menuList
            menuList.Add(init_page);
			menuList.Add(contact_page);
            menuList.Add(miPerfil_page);
            menuList.Add(salir_page);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
		}

		// Event for Menu Item selection, here we are going to handle navigation based
		// on user selection in menu ListView
		private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (evtItemSelected != null)
			{
				evtItemSelected(e.SelectedItem);
			}
			/*var item = (MasterPageItem)e.SelectedItem;
			Type page = item.TargetType;

			Detail = new NavigationPage((Page)Activator.CreateInstance(page));
			IsPresented = false;*/
		}
	}
}
