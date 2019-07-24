using ShootsDay.Managers;
using ShootsDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Models.Share
{
    class PhotoDetailViewModel : BaseViewModel
    {
        public Command Share { get; set; }
        public Command Download { get; set; }
        public ImageSource Source { get; set; }
        public Photoshoot photoshoot { get; set; }

        public PhotoDetailViewModel(Page context) : base(context)
        {
            Share = new Command(ShareCommand);
            Download = new Command(DownloadCommand);
        }

        void ShareCommand()
        {
            MessagingCenter.Send<ImageSource>(this.Source, "Share");
        }

        public void DownloadCommand()
        {
            try
            {
                MessagingCenter.Send<Photoshoot>(this.photoshoot, "Download");
            }
            catch (Exception e)
            {
                e = e;
            }            
        }
    }
}
