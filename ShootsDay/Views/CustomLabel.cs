using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Views
{
    class CustomLabel : Label
    {
        public static readonly BindableProperty IsUnderlinedProperty = BindableProperty.Create("IsUnderlined", typeof(bool), typeof(CustomLabel), false);

        public bool IsUnderlined
        {
            get { return (bool)GetValue(IsUnderlinedProperty); }
            set { SetValue(IsUnderlinedProperty, value); }
        }
    }
}
