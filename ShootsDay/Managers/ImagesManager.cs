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

        public string getProfilePicture(int idUser)
        {
            return Constants.PROFILE_IMAGE_URL + "/" + idUser + "/1.png";
        }

        public string gePhotoshootImage(string image)
        {
            return Constants.PHOTOSHOOTS_IMAGE_URL + "/" + Application.Current.Properties["id_event"]  + "/" + image;
        }

        public string gePhotoshootDetailImage(string urlImage)
        {
            return Constants.INIT_SERVER + urlImage;
        }

        public string getHomesImage(string image)
        {
            return Constants.REDSOCIAL_IMAGE_URL + "/" + Application.Current.Properties["id_event"] + "/" + image;
        }
    }
}
