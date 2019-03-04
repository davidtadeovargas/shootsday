using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Views
{
    class Loading
    {
        private static Loading instance = null;

        private bool shouldContinue = false; //Break for the loadingIndeterinate




        private Loading()
        {
        }

        public static Loading Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Loading();
                }
                return instance;
            }
        }    

        async private void loadingIndeterinate()
        {
            using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
            {
                while (shouldContinue)
                {
                    await Task.Delay(60);
                }
            }
        }


        async protected void loadingProgress(int limitSeconds)
        {
            using (IProgressDialog progress = UserDialogs.Instance.Progress("Progress", null, null, true, MaskType.Black))
            {
                for (int i = 0; i < limitSeconds; i++)
                {
                    progress.PercentComplete = i;
                    await Task.Delay(60);
                }
            }
        }

        /*
            End the loading process
             */
        public void closeLoading()
        {
            shouldContinue = false;
        }


        /*
            Start the loading process
             */
        public void showLoading()
        {
            shouldContinue = true;
            loadingIndeterinate();
        }
    }
}
