using ShootsDay.Managers;
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

            Device.BeginInvokeOnMainThread(() => {
                NameToolb.Text = SettingsManager.Instance.getUserName();
            });
        }

        private void OnLinkClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(Constants.CONTACT_URL));
        }

        public async void OnEmailClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + Constants.CONTACT_CONTACT_EMAIL));
        }

        public async void OnEmail2Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + Constants.CONTACT_CONTACT_EMAIL2));
        }
    }
}
