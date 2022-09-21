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

[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]

namespace Hermes
{

    [BroadcastReceiver(Name= "com.nonobo.hermes.BootBroadcastReceiver" ,Enabled = true, DirectBootAware = true, Permission = Manifest.Permission.ReceiveBootCompleted,Exported =true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.HighPriority, Categories = new[] { Intent.CategoryDefault })]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        private AndroidNotificationManager notificationManager=new AndroidNotificationManager();
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("Hermes", "Boot");
            if (SharedPrefrenceManager.CanStartService())
            {
                Log.Info("Hermes", "UserLogged");
                Intent i = new Intent(context.ApplicationContext, typeof(CommunicationService));
                context.ApplicationContext.StartForegroundService(i);
            }
            else
            {
                Log.Info("BootR", "UserNotLogged");
            }

        }
    }
}