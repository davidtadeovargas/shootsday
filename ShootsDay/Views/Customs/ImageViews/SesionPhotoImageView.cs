using Android.Content;
using Android.Util;
using Android.Widget;
using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootsDay.Views.Customs.ImageViews
{
    public class SesionPhotoImageView : ImageView
    {
        public Photoshoot photoshoot;




        public SesionPhotoImageView(Context context, IAttributeSet attr) : base(context, attr)
        {
            
        }
    }
}
