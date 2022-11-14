using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.DocumentFile.Provider;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hermes
{
    public class ChatsManager: Java.Lang.Object, IChildEventListener, INotifyPropertyChanged
    {
        private DatabaseReference mMessagesRef;
        private Dictionary<string, Chat> _chats;
        private bool started = false;
        private bool justSent = false;

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
            else
            {
                if (m.ImageUri != "" && m.ImageUri != null)
                {
                    Android.Net.Uri u = Android.Net.Uri.Parse(m.ImageUri);
                    string l = "";
                    App.StorageManager.GetFileLink(u.LastPathSegment, out l);
                    m.ImageLink = l;
                }

            }

            if (_chats.Keys.Contains(key))
            {
                //if (_chats[key].Messages[_chats[key].Messages.Count - 1].CompareIgnoreTimestamp(m)&&justSent)
                //{
                //    _chats[key].Messages.RemoveAt(_chats[key].Messages.Count - 1);
                //    justSent = false;
                //}
                _chats[key].Messages.Add(m);
            }
            else
            {
                _chats[key] = new Chat(key, new List<Message> { m });
            }


        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }
        private bool UploadMessageImage(Message m)
        {
            Android.Net.Uri uri = Android.Net.Uri.Parse(m.ImageUri);
            return App.StorageManager.UploadFile(uri, m.Recipient);
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
            //this._chats[m.Recipient].Messages.Add(m);
            //int index = this._chats[m.Recipient].Messages.Count - 1;
            Thread t = new Thread(new ThreadStart(delegate
            {
                Thread t = new Thread(new ThreadStart(delegate {
                    if (m.ImageUri != null && m.ImageUri != "")
                    {
                        UploadMessageImage(m);
                    }
                }));
                t.Start();
                if(m.ImageUri != null)
                {
                    m.IsImageSafe = MediaHelper.isSafe(Android.Net.Uri.Parse(m.ImageUri));
                }
                Java.Util.HashMap mess = m.ToHashMap();
                DatabaseReference dbRef = mMessagesRef.Child(m.Recipient).Push();
                Task sen = mMessagesRef.Child(m.Sender).Child(dbRef.Key).SetValue(mess);
                TaskHelper.AwaitAndroidTask(ref sen);
                t.Join();
                Task rec = dbRef.SetValue(mess);  
                TaskHelper.AwaitAndroidTask(ref rec);
                justSent = true;
                return;
            }));
            t.Start();
            return;
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