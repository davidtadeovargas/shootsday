using ShootsDay.Managers;
using ShootsDay.Models;
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
    public partial class RedSocial : ContentPage
    {
        public RedSocial()
        {
            init();
        }

        private void init()
        {
            InitializeComponent();

            BindingContext = new RedSocialViewModel(this);

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserLargeName();
                Title = "";
            });            
        }
    }
}