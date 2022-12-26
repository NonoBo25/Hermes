using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using Java.Lang;
using AndroidApp = Android.App.Application;

namespace Hermes
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
    public class AndroidNotificationManager 
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 1;
        int pendingIntentId = 1;

        NotificationManager manager;


        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }





        public Notification Create(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.AddFlags(ActivityFlags.NewTask);
            PendingIntent pendingIntent;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                pendingIntent = PendingIntent.GetActivity(AndroidApp.Context,
                        0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            }
            else
            {
                pendingIntent = PendingIntent.GetActivity(AndroidApp.Context,
                                        0, intent, PendingIntentFlags.UpdateCurrent);

            }

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            return builder.Build();
        }




        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

    }
}