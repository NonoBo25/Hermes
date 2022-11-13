using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.DocumentFile.Provider;
using Java.Interop;
using Java.Util;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class Message : IFirebaseObject
    {
        public Message() { }
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        
        public string Timestamp { get; set; }
        public string ImageLink { get; set; }
        public string ImageUri { get; set; }
        
        public void FromHashMap(HashMap map)
        {
            Content = map.Get("Content").ToString();
            Sender = map.Get("Sender").ToString();
            Recipient = map.Get("Recipient").ToString();
            Timestamp = map.Get("Timestamp").ToString();
            ImageLink = map.Get("ImageLink").ToString();
            ImageUri = map.Get("ImageUri").ToString();
        }

        

        public HashMap ToHashMap()
        {
            HashMap res = new HashMap();
            res.Put("Content", Content);
            res.Put("Sender", Sender);
            res.Put("Recipient", Recipient);
            res.Put("Timestamp", (Java.Lang.Object)Firebase.Database.ServerValue.Timestamp);
            res.Put("ImageLink", ImageLink);
            res.Put("ImageUri", ImageUri);
            return res;
        }
        
        public bool CompareIgnoreTimestamp(Message m)
        {
            return m.Content.Equals(Content) && m.Sender.Equals(Sender) && m.Recipient.Equals(Recipient) && m.ImageLink.Equals(ImageLink) && m.ImageUri.Equals(ImageUri);
        }


    }
}