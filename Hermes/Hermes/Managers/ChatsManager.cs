using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
    public class ChatsManager: Java.Lang.Object, IChildEventListener, INotifyPropertyChanged
    {
        private DatabaseReference mMessagesRef;
        private Dictionary<string, Chat> _chats;
        private bool started = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void Start()
        {
            if (!started)
            {
                mMessagesRef = FirebaseDatabase.Instance.GetReference("/messages");
                mMessagesRef.Child(App.AuthManager.CurrentUserUid).OrderByChild("timestamp").AddChildEventListener(this);
                started = true;
            }

            
        }
        public ChatsManager()
        {
            Log.Debug("Hermes", "created Chats Manager");
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
        public int ChatExists(string c)
        {
            if (_chats.Keys.Contains(c))
            {
                return _chats.Keys.ToList().IndexOf(c);
            }
            return -1;
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
        public void NewChat(string partner) 
        {
            _chats[partner] = new Chat(partner, new List<Message>());
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

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            Java.Util.HashMap d = snapshot.Value.JavaCast<Java.Util.HashMap>();
            Message m = new Message();
            m.FromHashMap(d);
            AddMessage(m);
            if (m.Sender != App.AuthManager.CurrentUserUid)
            {
                OnPropertyChanged("In");
            }
            else
            {
                OnPropertyChanged("Out");
            }
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName)
        {
            //throw new NotImplementedException();
        }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName)
        {
            //throw new NotImplementedException();
        }

        public void OnChildRemoved(DataSnapshot snapshot)
        {
            //throw new NotImplementedException();
        }
    }
}