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

            BindingContext = new UsersViewModel(this, usersList, noRecords);
        }        
    }
}