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
    public class Chat
    {
        private string partner;
        private List<Message> messages;

        public Chat(string partner, List<Message> messages)
        {
            this.partner = partner;
            this.messages = messages;
        }

        public string Partner { get => partner; set => partner = value; }
        public List<Message> Messages { get => messages; set => messages = value; }
    }
}