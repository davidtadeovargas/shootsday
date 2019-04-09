using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Models.Share
{
    class UserRegisterViewModel
    {
        public Command PermissionPicture { get; set; }
        public Photoshoot photoshoot { get; set; }




        public UserRegisterViewModel()
        {
            PermissionPicture = new Command(PictureCommand);
        }

        void PictureCommand()
        {
            try
            {
                MessagingCenter.Send<Object>("", "PictureCommand");
            }
            catch (Exception e)
            {

            }
            
        }
    }
}
