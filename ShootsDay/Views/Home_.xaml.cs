using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home_ : ContentPage
    {
        string title = "";




        public Home_()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("title_event"))
                title = Application.Current.Properties["title_event"].ToString();            
        }
        public void OnRedSocialClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MasterDetail(new Home_()));
        }

        private void OnInvitacionClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Abrir ventana de invitación");
            Navigation.PushModalAsync(new MasterDetail(new Invites()));
        }

        private void OnSesionFotosClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Abrir ventana de Sesion de fotos");
            Navigation.PushModalAsync(new MasterDetail(new Photoshoots()));
        }
    }
}