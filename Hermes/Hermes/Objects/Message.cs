using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    internal class Message : IFirebaseObject
    {
        private string _message;
        private string _sender;
        private string _recipient;
        private bool _hasMedia;
        private LinkedList<byte[]> _media;
        public Message(string message, string sender, string recipient)
        {
            _message = message;
            _sender = sender;
            _recipient = recipient;
            _hasMedia = false;
            _media = new LinkedList<byte[]>();
        }

        public void AddMedia(byte[] arr)
        {
            _hasMedia = true;
            _media.AddLast(arr);
        }

        public HashMap ToHashMap()
        {
            HashMap res = new HashMap();
            res.Put("message", _message);
            res.Put("sender", _sender);
            res.Put("recipient", _recipient);
            res.Put("hasMedia", _hasMedia);
            return res;
        }
    }
}