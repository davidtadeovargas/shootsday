﻿using System;
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
        static public string UPLOAD_PICTURE = SERVER + "pictures/add.json";
        static public string PHOTOSHOOTS = SERVER + "photoshoots.json";
        static public string USERS_PROFILE = SERVER + "users/profile/";
        static public string EVENT_USERS = SERVER + "events/getEventUsers.json";
        static public string USER_EVENTS = SERVER + "events/getUserEvents.json";        
        static public string REDSOCIAL = SERVER + "homes.json";
        static public string ADD_COMMENT = SERVER + "homes/add_comment.json";
        static public string GET_COMMENTS = SERVER + "homes/get_comments.json";
        static public string COMMENTS_ADD = SERVER + "comments/add.json";
        static public string LIKES_ADD = SERVER + "likes/add.json";
        static public string COMMENTS_GETLIST = SERVER + "comments/get_list_comments.json";
        static public string PICTURES_ADD = SERVER + "pictures/add.json";
        static public string USERS_LOGIN = SERVER + "users/login.json";
        static public string LIKE = SERVER + "likes/like.json";
        static public string USERS_REGISTER = SERVER + "users/register.json";

        static public string CONTACT_URL = "http://shootsday.com.mx/contacto";
        static public string INVITATION_IMAGE_URL = INIT_SERVER + "/invitations";
        static public string PROFILE_IMAGE_URL = INIT_SERVER + "/imgProfile";
        static public string PHOTOSHOOTS_IMAGE_URL = INIT_SERVER + "/photoShoots";
        static public string REDSOCIAL_IMAGE_URL = INIT_SERVER + "/redsocial";
        static public string CONTACT_CONTACT_EMAIL = "info@shootsday.com";
        static public string CONTACT_CONTACT_EMAIL2 = "jacqueline@shootsday.com";

        static public int LIMIT = 25; //25
    }
}
