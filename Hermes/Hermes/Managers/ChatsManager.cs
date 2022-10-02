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
        private DatabaseReference mRef;
        private Dictionary<string, Chat> _chats;

        public event PropertyChangedEventHandler PropertyChanged;
        public void Start()
        {
            mRef = FirebaseDatabase.Instance.GetReference("/inboxes").Child(FirebaseAuth.Instance.CurrentUser.Uid);
            mRef.AddValueEventListener(this);
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
            if (_chats.Keys.Contains(m.Sender))
            {
                _chats[m.Sender].Messages.Add(m);
            }
            else
            {
                _chats[m.Sender]=new Chat(m.Sender,new List<Message> { m });
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
                App.ChatsManager.AddMessage(m);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}