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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedSocial : ContentPage
    {
        public RedSocial()
        {
            InitializeComponent();

            BindingContext = new RedSocialViewModel(this);            
        }

        private void ViewProfileTapped(object sender, TappedEventArgs e)
        {
            User user = (User)e.Parameter;

            Profile_ Profile_ = new Profile_(user.id);            

            Navigation.PushModalAsync(Profile_);
        }
    }
}