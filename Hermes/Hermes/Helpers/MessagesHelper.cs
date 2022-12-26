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
    public static class MessagesHelper
    {
        public static void PreprocessIncomingMessage(ref Message m)
        {
            if (m.HasImage)
            {
                Android.Net.Uri u = Android.Net.Uri.Parse(m.ImageUri);
                string l = "";
                StorageManager.GetFileLink(u.LastPathSegment, out l);
                m.ImageLink = l;
            }
            
        }
        public static void PostProcessMessage(ref Message m)
        {
            if (m.Content == "")
            {
                m.Content = "\b";
            }
            if (m.ImageUri == "")
            {
                m.ImageUri = "\b";
            }
        }
        public static void TimeStampHashMap(ref Java.Util.HashMap map)
        {
            map.Put("Timestamp", (Java.Lang.Object)Firebase.Database.ServerValue.Timestamp);
        }
    }
}