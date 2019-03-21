using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Managers
{
    class SettingsManager
    {
        private static SettingsManager instance = null;

        private const string ISLOGGEDIN = "IsLoggedIn";
        private const string SUPERUSER = "superUser";
        private const string USER_ID = "user_id";
        private const string USERNAME = "username";
        private const string PASSWORD = "password";
        private const string ID_EVENT = "id_event";
        private const string HOST = "host";
        private const string ROLE_ID = "role_id";
        private const string TITTLE_EVENT = "title_event";


        
        						

        private SettingsManager()
        {
        }

        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsManager();
                }
                return instance;
            }
        }

        public void setTitleEvent(string titleEvent)
        {
            Application.Current.Properties[TITTLE_EVENT] = titleEvent;
        }
        public string getTitleEvent()
        {
            return (string)Application.Current.Properties[TITTLE_EVENT];
        }


        public void setRoleId(int roleId)
        {
            Application.Current.Properties[ROLE_ID] = roleId;
        }
        public int getRoleId()
        {
            return (int)Application.Current.Properties[ROLE_ID];
        }

        public void setHost(string host)
        {
            Application.Current.Properties[HOST] = host;
        }
        public string getHost()
        {
            return (string)Application.Current.Properties[HOST];
        }

        public void setIdEvent(int idEvent)
        {
            Application.Current.Properties[ID_EVENT] = idEvent;
        }
        public int getIdEvent()
        {
            return (int)Application.Current.Properties[ID_EVENT];
        }

        public void setPassword(string password)
        {
            Application.Current.Properties[PASSWORD] = password;
        }
        public string getPassword()
        {
            return (string)Application.Current.Properties[PASSWORD];
        }

        public void setUserName(string username)
        {
            Application.Current.Properties[USERNAME] = username;
        }
        public string getUserName()
        {
            return (string)Application.Current.Properties[USERNAME];
        }

        public void setUserId(int userId)
        {
            Application.Current.Properties[USER_ID] = userId;
        }
        public int getUserId()
        {
            return (int)Application.Current.Properties[USER_ID];
        }

        public void setIsSuperUser(bool val)
        {
            Application.Current.Properties[SUPERUSER] = val;
        }
        public bool isSuperUser()
        {
            return (bool)Application.Current.Properties[SUPERUSER];
        }

        public void setIsLoggedIn()
        {
            App.Current.Properties[ISLOGGEDIN] = true;
        }
        public SettingsManager setIsNotLoggedIn()
        {
            App.Current.Properties[ISLOGGEDIN] = false;
            return this;
        }
        public bool IsLoggedIn()
        {
            return (bool)Application.Current.Properties[ISLOGGEDIN];
        }

        public Task SavePropertiesAsync()
        {
            return Application.Current.SavePropertiesAsync();
        }
    }
}
