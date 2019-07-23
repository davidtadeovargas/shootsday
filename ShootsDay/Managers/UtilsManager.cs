using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Managers
{
    class UtilsManager
    {
        private static UtilsManager instance = null;
        private static readonly object padlock = new object();


        UtilsManager()
        {
        }

        public static UtilsManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UtilsManager();
                    }
                    return instance;
                }
            }
        }

        public Boolean isConectedToInternet(Page context, String message = "Al parecer no tienes conexion a internet :(\n\nRevisa tu conexion a Internet para poder continuar")
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await context.DisplayAlert("", message, "Entiendo");
                });
                return false;
            }
            return true;
        }
    }
}
