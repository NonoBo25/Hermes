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
    public class ImageMessageModel
    {
        private MessagesConnection _connection;
        private string _partner;
        private ImageData _image;

        public ImageMessageModel(string partner, Android.Net.Uri uri)
        {
            _partner = partner;
            _connection = new MessagesConnection();
            _image = new ImageData(uri);
        }

        public void SendMessage(string content)
        {

            Message m = new Message();
            m.Content = content;
            m.Sender = AuthManager.CurrentUserUid;
            m.Recipient = _partner;
            m.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            m.ImageUri = _image.ImageUri.ToString();
            m.IsImageSafe = _image.IsSafe;
            m.HasImage = true;
            _connection.SendMessage(m);
        }


    }
}