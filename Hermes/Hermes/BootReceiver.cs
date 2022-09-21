using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    [BroadcastReceiver(Enabled = true, Permission = Manifest.Permission.ReceiveBootCompleted,Exported =true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.HighPriority, Categories = new[] { Intent.CategoryDefault })]
    public class BootReceiver : BroadcastReceiver
    {
        private AndroidNotificationManager notificationManager=new AndroidNotificationManager();
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("Boot", "Boot");
            Intent i = new Intent(context.ApplicationContext, typeof(CommunicationService));
            i.AddFlags(ActivityFlags.NewTask);
            context.ApplicationContext.StartForegroundService(i);
        }
    }
}