using DLToolkit.Forms.Controls;
using ShootsDay.Managers;
using ShootsDay.Views;
using System.Diagnostics;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class App : Application
	{
        public string Language = "";
        public App()
		{
            var isLoggedIn = SettingsManager.Instance.IsLoggedIn();

            FlowListView.Init(); //Init library

            if (isLoggedIn)
                MainPage = new MasterDetail(new Home_());
            else
                MainPage = new UserLogin();
        }
        public void Logout()
        {
            //Properties.Clear();
            Properties.Remove("IsLoggedIn");
            //Properties["IsLoggedIn"] = false;
            MainPage = new UserLogin();
        }

        protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
