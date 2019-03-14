using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Models.Share
{
    class PhotoDetailViewModel
    {
        public Command Share { get; set; }
        public Command Download { get; set; }
        public ImageSource Source { get; set; }

        public PhotoDetailViewModel()
        {
            Share = new Command(ShareCommand);
            Download = new Command(DownloadCommand);
        }

        void ShareCommand()
        {
            MessagingCenter.Send<ImageSource>(this.Source, "Share");
        }

        void DownloadCommand()
        {
            MessagingCenter.Send<ImageSource>(this.Source, "Download");
        }
    }
}
