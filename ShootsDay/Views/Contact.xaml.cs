using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ShootsDay
{
	public partial class Contact : ContentPage
	{
		public Contact()
		{
			InitializeComponent();
		}

        private void OnContactoClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + Constants.CONTACT_CONTACT_EMAIL));
        }

        private void OnLinkClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(Constants.CONTACT_URL));
        }        
    }
}
