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

    [BroadcastReceiver(Name = "com.nonobo.hermes.BootBroadcastReceiver",Enabled =true,Exported =true)]
    [IntentFilter(new[] {Intent.ActionBootCompleted})]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("Hermes", "Boot");
            if (SharedPrefrenceManager.CanStartForegroundService())
            {
                App.init();
                SharedPrefrenceManager.StartForegroundService();
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