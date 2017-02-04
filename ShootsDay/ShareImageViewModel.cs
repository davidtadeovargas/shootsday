using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay
{
    public class ShareImageViewModel
    {
        public Command Share { get; set; }
        public ImageSource Source { get; set; }

        public ShareImageViewModel()
        {
            Share = new Command(ShareCommand);
        }

        void ShareCommand()
        {
            MessagingCenter.Send<ImageSource>(this.Source, "Share");
        }
    }
}
