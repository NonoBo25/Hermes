using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public enum UserEventType
    {
        UserAdded,
        UserRemoved,
        UserChanged
    }
    public class UserEventArgs:EventArgs
    {
        public UserEventType EventType { get; private set; }
        public string Uid { get; private set; }
        public string UserName { get; private set; }
        public UserEventArgs(UserEventType eventType, string uid,string username)
        {
            EventType = eventType;
            Uid = uid;
            UserName = username;
        }
    }
    public class UsersConnection : FirebaseLib.FirebaseDatabaseConnection<string>
    {
        private const string PATH = "/users";
        public event EventHandler<UserEventArgs> UserAdded;
        public UsersConnection() : base(PATH)
        {
        }
        protected override void OnCancelled(DatabaseException error)
        {
            throw new NotImplementedException();
        }
        protected override void OnChildAdded(string id, string newChild)
        {
            UserAdded?.Invoke(this, new UserEventArgs(UserEventType.UserAdded, id, newChild));
        }
        protected override void OnChildChanged(string id, string NewChild)
        {
            throw new NotImplementedException();
        }
        protected override void OnChildMoved(string id, string NewChild)
        {
            throw new NotImplementedException();
        }
        protected override void OnChildRemoved(string id, string removedChild)
        {
            throw new NotImplementedException();
        }
    }
}