using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ShootsDay.Views
{
    public class Profile : ContentPage
    {
        public Profile()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Perfil de Usuario" }
                }
            };
        }
    }
}
