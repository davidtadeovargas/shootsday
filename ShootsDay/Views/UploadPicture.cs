using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ShootsDay.Views
{
    public class UploadPicture : ContentPage
    {
        public UploadPicture()
        {
            Title = "Agregar foto";
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Button { Text = "Tomar foto" },
                    new Button { Text = "Subír foto desde a galería" }
                }
            };
        }
    }
}
