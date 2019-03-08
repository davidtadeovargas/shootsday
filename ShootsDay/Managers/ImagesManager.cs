using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Managers
{
    class ImagesManager
    {
        private static ImagesManager instance = null;
        private static readonly object padlock = new object();




        ImagesManager()
        {
        }

        public static ImagesManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ImagesManager();
                    }
                    return instance;
                }
            }
        }

        public string getInvitationMap()
        {
            return Constants.INVITATION_IMAGE_URL + "/" + Application.Current.Properties["id_event"] + ".png";
        }

        public string geProfilePicture(string image)
        {
            return Constants.PROFILE_IMAGE_URL + "/" + image;
        }
    }
}
