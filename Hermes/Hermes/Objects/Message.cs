using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.DocumentFile.Provider;
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
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        
        public string Timestamp { get; set; }
        public bool HasImage { get; private set; }
        public Image Image { get; set; }
        private Android.Net.Uri localFileUri;
        
        public void FromHashMap(HashMap map)
        {
            Content = map.Get("content").ToString();
            Sender = map.Get("sender").ToString();
            Recipient = map.Get("recipient").ToString();
            Timestamp = map.Get("timestamp").ToString();
            HasImage = bool.Parse(map.Get("hasimage").ToString());
            if (HasImage)
            {
                Image = new Image();
                Image.FromHashMap((HashMap)map.Get("image"));
            }

        }

        public HashMap ToHashMap()
        {
            HashMap res = new HashMap();
            res.Put("content", Content);
            res.Put("sender", Sender);
            res.Put("recipient", Recipient);
            res.Put("timestamp", (Java.Lang.Object)Firebase.Database.ServerValue.Timestamp);
            res.Put("hasimage", HasImage);
            if (HasImage)
            {
                res.Put("image", Image.ToHashMap());
            }
            
            return res;
        }
        public void AttachImage(Android.Net.Uri uri)
        {
            this.localFileUri = uri;
            this.HasImage=true;
            DocumentFile f = DocumentFile.FromSingleUri(Application.Context, uri);

            this.Image = new Image() { Name = f.Name }; 
        }
        public Android.Net.Uri getLocalUri()
        {
            return localFileUri;
        }

    }
}