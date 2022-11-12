using Android.App;
using Android.Content;
using Android.Graphics;
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
        private Dictionary<string, Bitmap> images;
        public Chat(string partner, List<Message> messages)
        {
            this.partner = partner;
            this.messages = messages;
            this.images = new Dictionary<string, Bitmap>();
        }

        public string Partner { get => partner; set => partner = value; }
        public List<Message> Messages { get => messages; set => messages = value; }
        public Dictionary<string, Bitmap> Images { get => images; set => images = value; }
    }
}