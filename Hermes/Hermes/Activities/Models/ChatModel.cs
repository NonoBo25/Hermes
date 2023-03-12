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
using System.Threading.Tasks;

namespace Hermes
{
    public class ImageData
    {
        public Android.Net.Uri ImageUri { get; set; }
        public bool IsSafe { get; private set; }
        public ImageData(Android.Net.Uri imageUri)
        {
            ImageUri = imageUri;
            IsSafe = MediaHelper.isSafe(ImageUri);
            
        }

    }
    public class NewMessageEventArgs : EventArgs
    {
        public Message NewMessage { get; private set; }
        public NewMessageEventArgs(Message m)
        {
            NewMessage = m;
        }
    }
    public class ChatModel
    {
        public event EventHandler<NewMessageEventArgs> NewMessage;
        private MessagesConnection _messagesConnection;
        private string _partner;
        
        public ChatModel(string partner)
        {
            _partner = partner;
            _messagesConnection = new MessagesConnection();
            _messagesConnection.MessageAdded += OnNewMessage;            
        }

        private void OnNewMessage(object sender, MessageEventArgs e)
        {
            Message m = e.Message;
            string partner = m.Type == MessageType.Outgoing ? m.Recipient : m.Sender;
            if (partner == _partner)
            {
                NewMessage?.Invoke(this,new NewMessageEventArgs(m));
            }
        }

        public void SendMessage(string content)
        {
            Message m = new Message();
            m.Content = content;
            m.Sender = AuthManager.CurrentUserUid;
            m.Recipient = _partner;
            m.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            m.HasImage = false;
            _messagesConnection.SendMessage(m);
        }
    }
}