using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
        public static AuthManager AuthManager = new AuthManager();
        public static UserManager UserManager = new UserManager();
        public static ChatsManager ChatsManager = new ChatsManager();


    }
}