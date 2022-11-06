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

namespace Hermes
{
    public class StorageManager
    {
        FirebaseStorage mStorage;
        public StorageManager()
        {
            mStorage = FirebaseStorage.Instance;
        }
        public bool UploadFile(Android.Net.Uri file,string uid)
        {
            DocumentFile f = DocumentFile.FromSingleUri(Application.Context, file);
            Task upload = mStorage.Reference.Child("files/" + uid + "/" + f.Name).PutFile(file);
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate { Android.Gms.Tasks.TasksClass.Await(upload); return; }));
                thr.Start();
                thr.Join();
                return upload.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }
        public bool GetFileLink(string fileName,out string link)
        {
            link = "";
            Task download = mStorage.Reference.Child("files/" + App.AuthManager.CurrentUserUid + "/" + fileName).GetDownloadUrl();
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate { Android.Gms.Tasks.TasksClass.Await(download); return; }));
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