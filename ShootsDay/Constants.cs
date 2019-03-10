using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootsDay
{
    class Constants
    {
        static public string INIT_SERVER = "http://shootsday.com.mx";
        static public string SERVER = INIT_SERVER + "/ws-jsproject/";
        static public string EVENTS = SERVER + "events.json";
        static public string MY_PICTURES = SERVER + "pictures/my_pictures.json";
        static public string PHOTOSHOOTS = SERVER + "photoshoots.json";
        static public string USERS_PROFILE = SERVER + "users/profile/";
        static public string HOMES = SERVER + "homes.json";
        static public string COMMENTS_ADD = SERVER + "comments/add.json";
        static public string LIKES_ADD = SERVER + "likes/add.json";
        static public string COMMENTS_GETLIST = SERVER + "comments/get_list_comments.json";
        static public string PICTURES_ADD = SERVER + "pictures/add.json";
        static public string USERS_LOGIN = SERVER + "users/login.json";
        static public string USERS_REGISTER = SERVER + "users/register.json";

        static public string CONTACT_URL = "http://shootsday.com.mx/contacto";
        static public string INVITATION_IMAGE_URL = INIT_SERVER + "/httpdocsimgEvent";
        static public string PROFILE_IMAGE_URL = INIT_SERVER + "/httpdocsimgProfile";
        static public string PHOTOSHOOTS_IMAGE_URL = INIT_SERVER + "/photoShoots";
        static public string CONTACT_CONTACT_EMAIL = "email@email.com";
    }
}
