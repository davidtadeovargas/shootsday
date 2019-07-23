using Plugin.Connectivity;
using ShootsDay.Managers;
using ShootsDay.ViewModels;
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

            /*
                Init the view model
             */
            var HomeViewModel = new HomeViewModel(this);
            BindingContext = HomeViewModel; //Attach the binding context

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = "Bienvenido, " + SettingsManager.Instance.getUserLargeName() + "!";
                eventLbl.Text = SettingsManager.Instance.getEventCode();
            });
        }

        public async void OnRedSocialClickedAsync(object sender, EventArgs e)
        {
            if (_userTapped)
                return;

            _userTapped = true;
            
            if (UtilsManager.Instance.isConectedToInternet(this)) //If connectivity error display message to user
            {
                await Navigation.PushModalAsync(new MasterDetail(new RedSocialTabPage()));
            }
            
            _userTapped = false;
        }       

        private async void OnSesionFotosClicked(object sender, EventArgs e)
        {
            if (_userTapped)
                return;            

            _userTapped = true;

            if (UtilsManager.Instance.isConectedToInternet(this)) //If connectivity error display message to user
            {
                Debug.WriteLine("Abrir ventana de Sesion de fotos");
                await Navigation.PushModalAsync(new MasterDetail(new PhotoSesionsPage_()));
            }
            
            _userTapped = false;
        }

        private async void OnInvitacionClicked(object sender, EventArgs e)
        {            
            if (_userTapped)
                return;

            _userTapped = true;

            if (UtilsManager.Instance.isConectedToInternet(this)) //If connectivity error display message to user
            {
                Debug.WriteLine("Abrir ventana de invitación");
                await Navigation.PushModalAsync(new MasterDetail(new Invites()));
            }
            
            _userTapped = false;
        }
    }
}