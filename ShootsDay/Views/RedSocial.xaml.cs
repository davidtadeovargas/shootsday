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
    }
}