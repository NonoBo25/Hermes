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
    public class CommunicationService : Service, IValueEventListener,IOnCompleteListener
    {
        private AndroidNotificationManager notificationManager;
        static readonly string TAG = typeof(CommunicationService).FullName;
        NotificationManager SysNotificationManager;

        private DatabaseReference _database;
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
                Login();
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

        public void OnCancelled(DatabaseError error)
        {
            Log.Info(TAG, "cancelled");
            Intent i = new Intent(this, typeof(CommunicationService));
            StartForegroundService(i);
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            Log.Info(TAG, "Datachange");
            Random r = new Random();
            SysNotificationManager.Notify(r.Next(0, 1000), notificationManager.Create("New Messages", "You Have New Messages"));
        }

        public void OnComplete(Task task)
        {
            Log.Info(TAG, "Login Complete");
            if (task.IsSuccessful)
            {
                Log.Info(TAG, "Login Success " + FirebaseAuth.Instance.CurrentUser.Email);
                InitDb();
            }
        }
        private void InitDb()
        {
            _database = FirebaseDatabase.Instance.GetReference("/inboxes").Child(FirebaseAuth.Instance.CurrentUser.Uid);
            _database.AddValueEventListener(this);
        }
        public void Login()
        {
            if (FirebaseAuth.Instance.CurrentUser == null)
            {
                UserData loggedUser = SharedPrefrenceManager.GetLoggedUser();
                if (!App.AuthManager.SignIn(loggedUser))
                {
                    Toast.MakeText(this, "Error SignIn", ToastLength.Long).Show();
                    StopForeground(true);
                    StopSelf();
                    return;
                }
            }
            InitDb();
        }

    }
}