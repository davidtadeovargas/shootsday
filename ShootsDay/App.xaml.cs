using System.Diagnostics;
using Xamarin.Forms;

namespace ShootsDay
{
	public partial class App : Application
	{
        public static App Current;
        public string Language = "";
        public App()
		{
            Current = this;
            //InitializeComponent();
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;

            if (isLoggedIn)
                MainPage = new InitApp();
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
