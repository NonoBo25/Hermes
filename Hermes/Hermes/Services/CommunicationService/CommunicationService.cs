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
            Intent intent = new Intent(this.ApplicationContext, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            PendingIntent pendingIntent;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                pendingIntent = PendingIntent.GetActivity(ApplicationContext,
                        0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            }
            else
            {
                pendingIntent = PendingIntent.GetActivity(ApplicationContext,
                                        0, intent, PendingIntentFlags.UpdateCurrent);

            }
            notificationManager = new AndroidNotificationManager();
            NotificationManager nm = (NotificationManager)GetSystemService(Context.NotificationService);


            NotificationCompat.Builder builder = new NotificationCompat.Builder(ApplicationContext, "default")
                .SetContentIntent(pendingIntent)
                .SetContentTitle("Service")
                .SetContentText("Started")
                .SetLargeIcon(BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
            StartForeground(1, builder.Build());
            
            Log.Info(TAG, "created notification Manager");
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                Login();
            }
            
            Log.Info(TAG, "complete");
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
            notificationManager.SendNotification("New Messages", "You Have New Messages");
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
            _database = FirebaseDatabase.Instance.GetReference("/test");//.Child(FirebaseAuth.Instance.CurrentUser.Uid);
            _database.AddValueEventListener(this);
        }
        public void Login()
        {
            if (FirebaseAuth.Instance.CurrentUser == null)
            {
                Log.Info(TAG, "User not logged");
                UserData loggedUser = SharedPrefrenceManager.GetLoggedUser();
                FirebaseAuth.Instance.SignInWithEmailAndPassword(loggedUser.Email, loggedUser.Password).AddOnCompleteListener(this);
                Log.Info(TAG, "User created login");
            }
            else
            {
                Log.Info(TAG, "User loggedin" + " " + FirebaseAuth.Instance.CurrentUser.Email);
                InitDb();
                Log.Info(TAG, "database connceted");
            }
        }

    }
}