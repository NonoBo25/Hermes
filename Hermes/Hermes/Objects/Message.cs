using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class Message : IFirebaseObject
    {
        public Message() { }

        public Message(string content, string sender, string recipient)
        {
            Content = content;
            Sender = sender;
            Recipient = recipient;
        }

        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        
        public void FromHashMap(HashMap map)
        {
            Content = map.Get("content").ToString();
            Sender = map.Get("sender").ToString();
            Recipient = map.Get("recipient").ToString();
        }

        public HashMap ToHashMap()
        {
            HashMap res = new HashMap();
            res.Put("content", Content);
            res.Put("sender", Sender);
            res.Put("recipient", Recipient);
            return res;
        }

    }
}