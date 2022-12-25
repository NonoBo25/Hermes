using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Storage;
using AndroidX.DocumentFile.Provider;
using Android.Gms.Tasks;
using System.Threading;
using Java.IO;
using System.IO;

namespace Hermes
{
    public static class StorageManager
    {
        static FirebaseStorage mStorage = FirebaseStorage.Instance;

        public static bool UploadFile(Android.Net.Uri file,string uid)
        {
            DocumentFile f = DocumentFile.FromSingleUri(Application.Context, file);
            Stream  s= Application.Context.ContentResolver.OpenInputStream(file);
            Task upload = mStorage.Reference.Child("files/" + uid + "/" + f.Name).PutStream(s);
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate { 
                    TasksClass.Await(upload); 
                    return; 
                }));
                thr.Start();
                thr.Join();
                return upload.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetFileLink(string fileName,out string link)
        {
            link = "";
            Task download = mStorage.Reference.Child("files/" + AuthManager.CurrentUserUid + "/" + fileName+".jpg").GetDownloadUrl(); 
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate {Android.Gms.Tasks.TasksClass.Await(download);return;}));
                thr.Start();
                thr.Join();
                if (download.IsSuccessful)
                {
                    link = download.Result.ToString();
                }

                return download.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }
         

    }
}