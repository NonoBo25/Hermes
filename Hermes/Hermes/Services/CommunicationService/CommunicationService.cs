using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    [Service(Name ="com.nonobo.hermes.CommunicationService",Enabled =true,Exported =true)]
    public class CommunicationService : Service,IOnCompleteListener
    {
        private AndroidNotificationManager notificationManager;
        static readonly string TAG = typeof(CommunicationService).FullName;
        NotificationManager SysNotificationManager;
        
        public IBinder Binder { get; private set; }
        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(TAG, "OnBind");
            this.Binder = new CommunicationBinder(this);
            return this.Binder;
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "Entered OnCreate");
            Notification n = ForegroundNotificationHelper.ReturnNotif();
            StartForeground(1,n);
            notificationManager = new AndroidNotificationManager();
            SysNotificationManager = (NotificationManager)this.GetSystemService(NotificationService);
            Log.Info(TAG, "created notification Manager");
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                if (App.AuthManager.SignIn(SharedPrefrenceManager.GetLoggedUser()))
                {
                    App.ChatsManager.Start();
                }
            }
            App.ChatsManager.PropertyChanged += ChatsManager_PropertyChanged;
            
        }

        private void ChatsManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Log.Debug(TAG, "New Message");
            if (e.PropertyName == "In")
            {
                Log.Debug(TAG, "New Incoming Message");
                Random r = new Random();
                SysNotificationManager.Notify(r.Next(0, 1000), notificationManager.Create("New Messages", "You Have New Messages"));
            }
        }

        public override bool OnUnbind(Intent intent)
        {
            Log.Debug(TAG, "OnUnbind");
            return base.OnUnbind(intent);
        }
        public override void OnDestroy()
        {
            Log.Info(TAG, "destroy");
            Intent i = new Intent(this, typeof(CommunicationService));
            StartForegroundService(i);
        }



        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                if (App.ChatsManager == null)
                {
                    App.ChatsManager = new ChatsManager();
                }
            }
        }


    }
}