
using Android.App;
using Android.Content;
using Java.IO;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootsDay
{
    public class DownloadImageFromUrl : Android.OS.AsyncTask<string, string, string>
    {
        private ProgressDialog pDialog;
        //private ImageView imgView;
        private Context context;
        public string Message { get; set; }
        public Action<string> OnImageDownloaded { get; set; }



        public DownloadImageFromUrl(Context context)
        {
            this.context = context;
            //this.imgView = imgView;
        }
        protected override void OnPreExecute()
        {
            pDialog = new ProgressDialog(context);
            pDialog.SetMessage(Message);
            pDialog.Indeterminate = false;
            pDialog.Max = 100;
            pDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pDialog.SetCancelable(true);
            pDialog.Show();
            base.OnPreExecute();
        }
        protected override void OnProgressUpdate(params string[] values)
        {
            base.OnProgressUpdate(values);
            pDialog.SetProgressNumberFormat(values[0]);
            pDialog.Progress = int.Parse(values[0]);
        }
        protected override void OnPostExecute(string result)
        {
            string strongPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string filePath = System.IO.Path.Combine(strongPath, milliseconds + ".jpg");
            pDialog.Dismiss();

            OnImageDownloaded(filePath);

            //imgView.SetImageDrawable(Drawable.CreateFromPath(filePath));
        }
        protected override string RunInBackground(params string[] @params)
        {
            string strongPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string filePath = System.IO.Path.Combine(strongPath, milliseconds + ".jpg");
            int count;
            try
            {
                URL url = new URL(@params[0]);
                URLConnection connection = url.OpenConnection();
                connection.Connect();
                int LengthOfFile = connection.ContentLength;
                InputStream input = new BufferedInputStream(url.OpenStream(), LengthOfFile);
                OutputStream output = new FileOutputStream(filePath);
                byte[] data = new byte[1024];
                long total = 0;
                while ((count = input.Read(data)) != -1)
                {
                    total += count;
                    PublishProgress("" + (int)((total / 100) / LengthOfFile));
                    output.Write(data, 0, count);
                }
                output.Flush();
                output.Close();
                input.Close();
            }
            catch (Exception e)
            {
            }
            return null;
        }
    }
}
