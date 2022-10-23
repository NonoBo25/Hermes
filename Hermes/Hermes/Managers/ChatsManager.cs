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
            mMessagesRef.Child(App.AuthManager.CurrentUserUid).OrderByChild("timestamp").AddValueEventListener(this);
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
            if (m.Sender.Equals(App.AuthManager.CurrentUserUid))
            {
                key = m.Recipient;
            }
            if (_chats.Keys.Contains(key))
            {
                _chats[key].Messages.Add(m);
            }
            else
            {
                _chats[key]=new Chat(key,new List<Message> { m });
            }
        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            _chats.Clear();

            foreach (DataSnapshot i in snapshot.Children.ToEnumerable())
            {
                Java.Util.HashMap d = i.Value.JavaCast<Java.Util.HashMap>();
                Message m = new Message();
                m.FromHashMap(d);
                m.Timestamp = i.Key.ToString();
                AddMessage(m);
            }
            OnPropertyChanged(nameof(ChatList));

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendMessage(Message m) {   
            Java.Util.HashMap mess = m.ToHashMap();
            DatabaseReference dbRef = mMessagesRef.Child(m.Recipient).Push();
            dbRef.SetValue(mess);
            mMessagesRef.Child(m.Sender).Child(dbRef.Key).SetValue(mess);
        }

    }
}