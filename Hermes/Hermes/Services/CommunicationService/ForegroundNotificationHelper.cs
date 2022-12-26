using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    internal class ForegroundNotificationHelper
    {
        private static string foregroundChannelId = "9001";
        private static Context context = global::Android.App.Application.Context;


        public static Notification ReturnNotif()
        {
            // Building intent
            Intent intent = new Intent(context, typeof(MainActivity));
            PendingIntent pendingIntent;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                pendingIntent = PendingIntent.GetActivity(context,
                        0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            }
            else
            {
                pendingIntent = PendingIntent.GetActivity(context,
                                        0, intent, PendingIntentFlags.UpdateCurrent);

            }

            var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
                .SetContentTitle("Hermes")
                .SetContentText("Foreground Service Started")
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            // Building channel if API verion is 26 or above
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "Title", NotificationImportance.High);
                var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }
    }
}