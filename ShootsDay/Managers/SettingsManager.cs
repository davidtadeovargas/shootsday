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
        private const string USERNAME_LARGE = "usernamelarge";
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
            if (Application.Current.Properties.ContainsKey(TITTLE_EVENT))
            {
                var result = (string)Application.Current.Properties[TITTLE_EVENT];
                return result;
            }
            else
            {
                return null;
            }            
        }


        public void setRoleId(int roleId)
        {
            Application.Current.Properties[ROLE_ID] = roleId;
        }
        public int getRoleId()
        {
            if (Application.Current.Properties.ContainsKey(ROLE_ID))
            {
                var result = (int)Application.Current.Properties[ROLE_ID];
                return result;
            }
            else
            {
                return -1;
            }
        }

        public void setHost(string host)
        {
            Application.Current.Properties[HOST] = host;
        }
        public string getHost()
        {
            if (Application.Current.Properties.ContainsKey(HOST))
            {
                var result = (string)Application.Current.Properties[HOST];
                return result;
            }
            else
            {
                return null;
            }
        }

        public void setIdEvent(int idEvent)
        {
            Application.Current.Properties[ID_EVENT] = idEvent;
        }
        public int getIdEvent()
        {
            if (Application.Current.Properties.ContainsKey(ID_EVENT))
            {
                var result = (int)Application.Current.Properties[ID_EVENT];
                return result;
            }
            else
            {
                return -1;
            }
        }

        public void setPassword(string password)
        {
            Application.Current.Properties[PASSWORD] = password;
        }
        public string getPassword()
        {
            if (Application.Current.Properties.ContainsKey(PASSWORD))
            {
                var result = (string)Application.Current.Properties[PASSWORD];
                return result;
            }
            else
            {
                return null;
            }
        }

        public void setUserName(string username)
        {
            Application.Current.Properties[USERNAME] = username;
        }
        public string getUserName()
        {
            if (Application.Current.Properties.ContainsKey(USERNAME))
            {
                var result = (string)Application.Current.Properties[USERNAME];
                return result;
            }
            else
            {
                return null;
            }
        }

        public void setUserLargeName(string usernameLarge)
        {
            Application.Current.Properties[USERNAME_LARGE] = usernameLarge;
        }
        public string getUserLargeName()
        {
            if (Application.Current.Properties.ContainsKey(USERNAME_LARGE))
            {
                var result = (string)Application.Current.Properties[USERNAME_LARGE];
                return result;
            }
            else
            {
                return null;
            }
        }

        public void setUserId(int userId)
        {
            Application.Current.Properties[USER_ID] = userId;
        }
        public int getUserId()
        {
            var result = -1;
            if (Application.Current.Properties.ContainsKey(USER_ID))
            {
                result = (int)Application.Current.Properties[USER_ID];
            }
            return result;
        }

        public void setIsSuperUser(bool val)
        {
            Application.Current.Properties[SUPERUSER] = val;
        }
        public bool isSuperUser()
        {
            var result = true;
            if (Application.Current.Properties.ContainsKey(SUPERUSER))
            {
                result = (bool)Application.Current.Properties[SUPERUSER];
            }
            return result;
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
            var result = false;        
            if (Application.Current.Properties.ContainsKey(ISLOGGEDIN)) {
                result = (bool)Application.Current.Properties[ISLOGGEDIN];
            }
            return result;
        }

        public Task SavePropertiesAsync()
        {
            return Application.Current.SavePropertiesAsync();
        }
    }
}
