using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class ChatsManager: Java.Lang.Object, IValueEventListener, INotifyPropertyChanged
    {
        private DatabaseReference mMessagesRef;
        private Dictionary<string, Chat> _chats;

        public event PropertyChangedEventHandler PropertyChanged;
        public void Start()
        {
            mMessagesRef = FirebaseDatabase.Instance.GetReference("/messages");
            mMessagesRef.Child(FirebaseAuth.Instance.CurrentUser.Uid).AddValueEventListener(this);
        }
        public ChatsManager()
        {
            _chats = new Dictionary<string, Chat>();
        }
        public ChatsManager(List<Message> l)
        {
            _chats = new Dictionary<string, Chat>();
            foreach(Message i in l)
            {
                AddMessage(i);
            }
        }
        public List<Chat> ChatList { get => _chats.Values.ToList(); }

        public List<Chat> GetListOfChats()
        {
            return _chats.Values.ToList();
        }
        
        public void AddMessage(Message m)
        {
            string key=m.Sender;
            if (m.Sender.Equals(FirebaseAuth.Instance.CurrentUser.Uid))
            {
                key = m.Recipient;
            }
            if (_chats.Keys.Contains(key))
            {
                _chats[key].Messages.Add(m);
            }
            else
            {
                _chats[key]=new Chat(m.Sender,new List<Message> { m });
            }
            OnPropertyChanged(nameof(ChatList));
        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            foreach (DataSnapshot i in snapshot.Children.ToEnumerable())
            {
                Java.Util.HashMap d = i.Value.JavaCast<Java.Util.HashMap>();
                Message m = new Message();
                m.FromHashMap(d);
                AddMessage(m);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendMessage(Message m) {
            long tStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            mMessagesRef.Child(m.Recipient).Child(tStamp.ToString()).SetValue(m.ToHashMap());
            mMessagesRef.Child(m.Sender).Child(tStamp.ToString()).SetValue(m.ToHashMap());
        }
    }
}