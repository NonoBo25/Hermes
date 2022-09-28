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
        public Message(string message, string sender, string recipient)
        {
            _message = message;
            _sender = sender;
            _recipient = recipient;
        }
        public HashMap ToHashMap()
        {
            HashMap res = new HashMap();
            res.Put("message", _message);
            res.Put("sender", _sender);
            res.Put("recipient", _recipient);
            return res;
        }
    }
}