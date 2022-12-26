using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hermes
{
    public static class TaskHelper
    {
        public static void AwaitAndroidTask(ref Task t)
        {
            Task t1 = t;
            Thread thr = new Thread(new ThreadStart(delegate { Android.Gms.Tasks.TasksClass.Await(t1); return; }));
            thr.Start();
            thr.Join();
            t = t1;
            return;
        }

    }
}