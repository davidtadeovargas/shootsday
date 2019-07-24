using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Acr.UserDialogs;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using ShootsDay.Models;
using System.Net;

namespace ShootsDay.Droid
{
	[Activity(Label = "ShootsDay.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        const int ShareImageId = 1000;
        const int REQUEST_DOWNLOAD_FILE = 100;
        const int REQUEST_PICTURE = 101;

        Photoshoot photoshootCurrent = null;




        protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

            /* In Android N+ there are no more file:// URIs, only content:// URIs instead, this fix that*/
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.OnCreate(bundle);

            UserDialogs.Init(this); //Initialize the dialogs

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();

            LoadApplication(new App());

            MessagingCenter.Subscribe<ImageSource>(this, "Share", Share, null);
            MessagingCenter.Subscribe<Photoshoot>(this, "Download", DownloadCommand, null);
            MessagingCenter.Subscribe<Object>(this, "KeyboardClick", KeyboardClick, null);
            MessagingCenter.Subscribe<Object>(this, "PictureCommand", PictureCommand, null);
        }


        /*
         Download image
             */
        void DownloadCommand(Photoshoot photoshoot)
        {
            photoshootCurrent = photoshoot;

            bool readPermission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted;
            bool writePermission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted;
            if (readPermission && writePermission)
            {
                // We have permission, go ahead 
                downloadFile();
            }
            else
            {
                // Permission is not granted. If necessary display rationale & request.
                var requiredPermissions = new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };

                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle(Resource.String.Pemissions);
                alert.SetMessage(Resource.String.PermissionNeeded);
                alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) =>
                {
                    ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_DOWNLOAD_FILE);
                });
                alert.Show();
            }
        }


        public Android.Views.View root;
        public void KeyboardClick(Object Object)
        {
            if (root == null)
            {
                root = FindViewById<Android.Views.View>(Android.Resource.Id.Content);
            }
            root.PlaySoundEffect(SoundEffects.Click);
        }


        async void PictureCommand(Object Object)
        {
            try
            {
                bool readPermission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted;
                bool writePermission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted;
                if (readPermission && writePermission)
                {
                    // We have permission, go ahead 

                }
                else
                {
                    // Permission is not granted. If necessary display rationale & request.
                    //var requiredPermissions = new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    var requiredPermissions = new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };

                    ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_PICTURE);
                }
            }
            catch (Exception e)
            {

            }            
        }

        async void Share(ImageSource imageSource)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("image/png");

            var handler = new ImageLoaderSourceHandler();
            var bitmap = await handler.LoadImageAsync(imageSource, this);
            
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; //Miliseconds to do not repeat the file name

            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads
                + Java.IO.File.Separator + milliseconds + ".png");

            using (var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }

            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));

            var intentChooser = Intent.CreateChooser(intent, "Share via");

            StartActivityForResult(intentChooser, ShareImageId);

            KeyboardClick(imageSource);
        }


        /*
            Download file
             */
        private void downloadFile()
        {
            try
            {
                WebClient webClient = new WebClient(); webClient.DownloadFile(photoshootCurrent.url_image, "/sdcard/download/Test.xls");
                DownloadImageFromUrl download = new DownloadImageFromUrl(this);
                download.Message = Resources.GetString(Resource.String.DownloadingFile);
                download.OnImageDownloaded += OnImageDownloaded;
                download.Execute(photoshootCurrent.url_image);
            }
            catch (Exception e)
            {
                e = e;
            }            
        }

        /*
         Callback when image it is already downloaded
             */
        void OnImageDownloaded(string path)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("");
            alert.SetMessage(Resources.GetString(Resource.String.DownloadedFile));
            alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) => {                
            });
            alert.Show();
        }

        /*
            Request permissions callback
             */
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                /*
                Download a file
             */
                if (requestCode == REQUEST_DOWNLOAD_FILE)
                {
                    // Check if the required permission has been granted
                    if (grantResults[0] == Permission.Granted && grantResults[0] == Permission.Granted)
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle(Resource.String.Pemissions);
                        alert.SetMessage(Resource.String.PermissionGranted);
                        alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) => {
                            downloadFile();
                        });
                        alert.Show();
                    }
                    else
                    {
                        //Not granted                    
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle(Resource.String.Pemissions);
                        alert.SetMessage(Resource.String.PermissionNeeded);
                        alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) => {
                        });
                        alert.Show();
                    }

                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                }
                else if (requestCode == REQUEST_PICTURE)
                {
                    // Check if the required permission has been granted
                    if (grantResults[0] == Permission.Granted && grantResults[0] == Permission.Granted)
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle(Resource.String.Pemissions);
                        alert.SetMessage(Resource.String.PermissionGranted);
                        alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) => {

                        });
                        alert.Show();
                    }
                    else
                    {
                        //Not granted                    
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle(Resource.String.Pemissions);
                        alert.SetMessage(Resource.String.PermissionNeeded);
                        alert.SetPositiveButton(Resource.String.OK, (senderAlert, args) => {
                        });
                        alert.Show();
                    }

                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                }                
            }
            catch (Exception e)
            {
                e = e;
            }            
        }
    }
}
