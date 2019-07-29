using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShootsDay.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace ShootsDay.Droid
{
    
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string fileName = Path.GetFileName(url);
                var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                var absolutePath = pathFile.AbsolutePath;
                webClient.DownloadFileAsync(new Uri(url), Path.Combine(absolutePath, fileName));
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {            
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }
}