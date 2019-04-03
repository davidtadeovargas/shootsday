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
    public partial class EventUsers : ContentPage
    {
        public EventUsers()
        {
            InitializeComponent();

            BindingContext = new UsersViewModel(this);
        }

        private void ViewProfileTapped(object sender, TappedEventArgs e)
        {
            int userId = (int)e.Parameter;

            Profile_ Profile_ = new Profile_(userId);

            Navigation.PushModalAsync(Profile_);
        }
    }
}