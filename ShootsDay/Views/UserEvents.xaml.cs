using ShootsDay.Managers;
using ShootsDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootsDay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEvents : ContentPage
    {
        public UserEvents()
        {
            InitializeComponent();

            BindingContext = new UserEventsViewModel(this);

            Device.BeginInvokeOnMainThread(() => {
                //NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
            });
        }        
    }
}