﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ShootsDay.Models;
using ShootsDay.Views;
using Xamarin.Forms;

namespace ShootsDay
{
	public class MasterDetail : MasterDetailPage
	{
		public User usuario;
		MenuPage menuPage;

        Type previousPage;

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
				BarBackgroundColor = Color.FromHex("#1976D3"),
				BarTextColor = Color.White                
			};
		}

		private async void MenuPage_EvtItemSelected(object sender, object item)
		{
            if (item == null)
            {
                return;
            }

			var itemSelect = (MasterPageItem)item;            
            Type page = itemSelect.TargetType;
            IsPresented = false;

            if (itemSelect.Title == "Cerrar sesión")
            {
                /*
                    Question before to continue
                 */
                var answer = await DisplayAlert("", "¿Seguro que que quieres cerrar sesion?", "Si", "No");
                if (!answer)
                {
                    var listView = ((ListView)sender);

                    // clear selected item
                    listView.SelectedItem = null;

                    return;
                }                
            }

            previousPage = page;

            if (itemSelect.Title == "Inicio" || 
                itemSelect.Title == "Invitacion" || 
                itemSelect.Title == "Sesion de Fotos" || 
                itemSelect.Title == "Contacto" ||
                itemSelect.Title == "Usuarios" ||
                itemSelect.Title == "Mi perfil")
            {
                Application.Current.MainPage = new MasterDetail((Page)Activator.CreateInstance(page));
            }
            else if (itemSelect.Title == "Cerrar sesión")
            {
                Application.Current.MainPage = (Page)Activator.CreateInstance(page);
            }
            else
            {
                Detail.Navigation.PushAsync((Page)Activator.CreateInstance(page));
            }

            /*Detail = new NavigationPage((Page)Activator.CreateInstance(page))
            {
                BarBackgroundColor = Color.FromHex("#bababa"),
                BarTextColor = Color.White
            };*/

            IsPresented = false;
        }
	}
}