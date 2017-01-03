using Xamarin.Forms;

namespace ShootsDay
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new ShootsDayPage();
			//MainPage = new MainPage();
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
