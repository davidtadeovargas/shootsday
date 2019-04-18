using ShootsDay.Managers;
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
        private bool _userTapped;





        public Home_()
        {
            init();
        }


        private void init()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("title_event"))
                title = Application.Current.Properties["title_event"].ToString();

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
            });
        }

        public async Task OnRedSocialClickedAsync(object sender, EventArgs e)
        {
            if (_userTapped)
                return;

            _userTapped = true;

            await Navigation.PushModalAsync(new MasterDetail(new RedSocialTabPage()));
            _userTapped = false;
        }

        private async void OnSesionFotosClicked(object sender, EventArgs e)
        {
            if (_userTapped)
                return;

            _userTapped = true;

            Debug.WriteLine("Abrir ventana de Sesion de fotos");
            await Navigation.PushModalAsync(new MasterDetail(new PhotoSesionsPage_()));
            _userTapped = false;
        }

        private async void OnInvitacionClicked(object sender, EventArgs e)
        {            
            if (_userTapped)
                return;

            _userTapped = true;

            Debug.WriteLine("Abrir ventana de invitación");
            await Navigation.PushModalAsync(new MasterDetail(new Invites()));
            _userTapped = false;
        }
    }
}