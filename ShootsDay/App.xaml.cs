using Xamarin.Forms;

namespace ShootsDay
{
	public partial class App : Application
	{
		public const string Usuario = "username";
		private const string Contrasena = "password";
		private const string EventoId = "id_event";

		public App()
		{
			InitializeComponent();

			//MainPage = new ShootsDayPage();
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

		public string username
		{
			get
			{
				if (Properties.ContainsKey(Usuario))
					return Properties[Usuario].ToString();

				return "";
			}

			set
			{
				Properties[Usuario] = value;
			}
		}
	}
}
