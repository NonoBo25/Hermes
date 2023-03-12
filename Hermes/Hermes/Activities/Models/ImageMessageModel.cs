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
using System.Threading;

namespace Hermes
{
    public class ImageMessageModel
    {
        private MessagesConnection _connection;
        private string _partner;
        private ImageData _image;
        private bool _isClassified;
        public ImageMessageModel(string partner, Android.Net.Uri uri)
        {
            _isClassified = false;
            _partner = partner;
            _connection = new MessagesConnection();
            Thread t = new Thread(new ThreadStart(delegate { 
                _image = new ImageData(uri);
                _isClassified = true;
            }));
            t.Start();
        }

        public void SendMessage(string content)
        {
            Thread t = new Thread(new ThreadStart(delegate
            {
                while (!_isClassified)
                { }
                Message m = new Message();
                m.Content = content;
                m.Sender = AuthManager.CurrentUserUid;
                m.Recipient = _partner;
                m.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                m.ImageUri = _image.ImageUri.ToString();
                m.IsImageSafe = _image.IsSafe;
                m.HasImage = true;
                _connection.SendMessage(m);
             
            }));
            t.Start();
        }


    }
}