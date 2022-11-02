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
    public static class App
    {
        public static TimeManager TimeManager;
        public static AuthManager AuthManager;
        public static UserManager UserManager;
        public static ChatsManager ChatsManager;
        public static StorageManager StorageManager;
        private static bool initialized = false;
        public static void init()
        {
            if (!initialized)
            {
                Log.Debug("HERMES-APP", "INIT");
                TimeManager = new TimeManager();
                AuthManager = new AuthManager();
                UserManager = new UserManager();
                ChatsManager = new ChatsManager();
                StorageManager = new StorageManager();
                initialized= true;
            }

        }
    }
}