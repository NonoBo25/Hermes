using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.DocumentFile.Provider;
using Java.Interop;
using Java.Util;
using Newtonsoft.Json;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{

    public enum MessageType
    {
        Incoming,
        Outgoing,
    }

    public class Message
    {
        public Message() { }

        public string Content;
        public string Sender;
        public string Recipient;
        public string Timestamp;
        public string ImageUri;
        public bool IsImageSafe;
        public MessageType Type;

        public bool HasImage;

        public string ImageLink;
    }

}