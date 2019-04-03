using ShootsDay.Managers;
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
			var init_page = new MasterPageItem() { Title = "Inicio", Icon = "inicio.png", TargetType = typeof(Home_) };
            var misfotos_page = new MasterPageItem() { Title = "Red Social", Icon = "misfotos.png", TargetType = typeof(RedSocial) };
            var invitacion_page = new MasterPageItem() { Title = "Invitacion", Icon = "invitacion_.png", TargetType = typeof(Invites) };
            var sesionFotos_page = new MasterPageItem() { Title = "Sesion de Fotos", Icon = "sesionfotos_.png", TargetType = typeof(PhotoSesionsPage_) };
            var contact_page = new MasterPageItem() { Title = "Contacto", Icon = "contacto.png", TargetType = typeof(Contact) };

            var autorizarFotos_page = new MasterPageItem() { Title = "Autorizar fotos", TargetType = typeof(Profile_) };
            var eventos_page = new MasterPageItem() { Title = "Eventos", TargetType = typeof(Profile_) };
            var usuarios_page = new MasterPageItem() { Title = "Usuarios", TargetType = typeof(Profile_) };

            var miPerfil_page = new MasterPageItem() { Title = "Mi perfil", TargetType = typeof(Profile_) };
            var salir_page = new MasterPageItem() { Title = "Cerrar sesión", TargetType = typeof(UserLogin) };

            // Adding menu items to menuList
            menuList.Add(init_page);
            menuList.Add(misfotos_page);
            menuList.Add(invitacion_page);
            menuList.Add(sesionFotos_page);
            menuList.Add(contact_page);

            if (SettingsManager.Instance.isSuperUser())
            {
                menuList.Add(autorizarFotos_page);
            }
            
            menuList.Add(eventos_page);
            menuList.Add(usuarios_page);
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

			var item = (MasterPageItem)e.SelectedItem;
			Type page = item.TargetType;
		}
	}
}
