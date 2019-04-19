using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ShootsDay.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            
            LoadApplication(new App());

            MessagingCenter.Subscribe<ImageSource>(this, "Share", Share, null);

            return base.FinishedLaunching(app, options);
		}

        async void Share(ImageSource imageSource)
        {
            var handler = new ImageLoaderSourceHandler();
            var uiImage = await handler.LoadImageAsync(imageSource);

            var item = NSObject.FromObject(uiImage);
            var activityItems = new[] { item };
            var activityController = new UIActivityViewController(activityItems, null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
        }
    }
}
