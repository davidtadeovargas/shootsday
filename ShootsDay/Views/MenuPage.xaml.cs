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
			var page1 = new MasterPageItem() { Title = "Inicio", Icon = "home.png", TargetType = typeof(Home) };
			var page2 = new MasterPageItem() { Title = "Mis fotos", Icon = "picture.png", TargetType = typeof(MyPictures) };
			var page3 = new MasterPageItem() { Title = "Invitación", Icon = "invite.png", TargetType = typeof(Invites) };
			var page4 = new MasterPageItem() { Title = "Sesión de fotos", Icon = "photoshoot.png", TargetType = typeof(Photoshoots) };
			var page5 = new MasterPageItem() { Title = "Contacto", Icon = "contact.png", TargetType = typeof(Contact) };

			// Adding menu items to menuList
			menuList.Add(page1);
			menuList.Add(page2);
			menuList.Add(page3);
			menuList.Add(page4);
			menuList.Add(page5);

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
