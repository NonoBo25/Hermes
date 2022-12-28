using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Hermes
{
    public enum MessageEventType
    {
        NewMessage,
        MessageDeleted,
        MessageUpdated
    }
    public class MessageEventArgs
    {
        public MessageEventType EventType { get; private set; }
        public Message Message { get; private set; }
        public MessageEventArgs(MessageEventType eventType, Message m)
        {
            EventType = eventType;
            Message = m;
        }
    }
    public class MessagesConnection : FirebaseLib.FirebaseDatabaseConnection<Message>
    {
        private const string PATH = "/messages/";
        public event EventHandler<MessageEventArgs> MessageAdded;
        public event EventHandler<MessageEventArgs> IncomingMessage;
        public event EventHandler<MessageEventArgs> OutgoingMessage;
        public event EventHandler MessageDeleted;
        public event EventHandler MessageUpdated;
        private string _partner;

        
        public MessagesConnection() : base(PATH+AuthManager.CurrentUserUid)
        {
            _partner = null;
        }
        public MessagesConnection(string partner) : base(PATH + AuthManager.CurrentUserUid)
        {
            _partner = partner;
        }
        private void OnNewMessage(Message m)
        {
            if (_partner != null)
            {
                if (m.GetPartner() != _partner)
                {
                    return;
                }
            }
            switch (m.Type)
            {
                case MessageType.Incoming:
                    MessagesHelper.PreprocessIncomingMessage(ref m);
                    IncomingMessage?.Invoke(this, new MessageEventArgs(MessageEventType.NewMessage, m));
                    break;
                case MessageType.Outgoing:
                    OutgoingMessage?.Invoke(this, new MessageEventArgs(MessageEventType.NewMessage, m));
                    break;
                default:
                    break;
            }
            MessageAdded?.Invoke(this, new MessageEventArgs(MessageEventType.NewMessage, m));
        }
        protected override void OnCancelled(DatabaseException error)
        {
            
        }

        protected override void OnChildAdded(string id,Message newChild)
        {
            OnNewMessage(newChild);
        }
        protected override void OnChildChanged(string id, Message NewChild)
        {
            
        }
        protected override void OnChildMoved(string id, Message NewChild)
        {
        }
        protected override void OnChildRemoved(string id, Message removedChild)
        {
        }
        private bool UploadMessageImage(Message m)
        {
            Android.Net.Uri uri = Android.Net.Uri.Parse(new String(m.ImageUri));
            return StorageManager.UploadFile(uri, m.Recipient);
        }
        private void SendMessageToDest(DatabaseReference dest,Message m)
        {
            MessagesHelper.PostProcessMessage(ref m);
            Java.Util.HashMap map = FirebaseLib.JavaCSHelper.ObjectToHashMap(m);
            MessagesHelper.TimeStampHashMap(ref map);
            Task sen = dest.Push().SetValue(map);
            TaskHelper.AwaitAndroidTask(ref sen);
        }
        private void sendMessage(Message m)
        {
            DatabaseReference RecipientRefrence = FirebaseDatabase.Instance.GetReference(PATH + m.Recipient);
            m.Type = MessageType.Outgoing;
            SendMessageToDest(_ref, m);
            if (m.ImageUri != null && m.ImageUri != "")
            {
                Thread t = new Thread(new ThreadStart(delegate { UploadMessageImage(m); }));
                t.Start();
                t.Join();
            }
            m.Type = MessageType.Incoming;
            SendMessageToDest(RecipientRefrence, m);
            return;
        }
        public void SendMessage(Message m)
        {
            Thread t = new Thread(new ThreadStart(delegate{ sendMessage(m); }));
            t.Start();
            return;
        }
    }
}