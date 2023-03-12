using Android.Runtime;
using Firebase.Database;
using Google.Gson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace FirebaseLib
{
    static class JavaCSHelper
    {

        public static Java.Util.HashMap ObjectToHashMap(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            Java.Util.HashMap map = new Java.Util.HashMap(JsonConvert.DeserializeObject<Dictionary<string, object>>(json));
            return map;
        }

        public static T HashMapToObject<T>(Java.Util.HashMap map)
        {
            Gson gson = new Gson();
            String json = gson.ToJson(map);
            T myobject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return myobject;
        }
    }
    public static class FirebaseHelper
    {
        public static T SnapshotToObject<T>(DataSnapshot snap) 
        {
            try
            {
                Java.Util.HashMap d = snap.Value.JavaCast<Java.Util.HashMap>();
                return JavaCSHelper.HashMapToObject<T>(d);
            }
            catch
            {
                return (T)Convert.ChangeType(snap.Value.ToString(), typeof(T));
            }

        }
    }

    public class FirebaseDatabaseErrorEventArgs : EventArgs
    {

        public DatabaseError Error { get; private set; }
        public FirebaseDatabaseErrorEventArgs(DatabaseError error)
        {
            Error = error;
        }

    }
    public abstract class FirebaseDatabaseConnection<T> : Java.Lang.Object, IChildEventListener
    {
        protected DatabaseReference _ref;
        event EventHandler<FirebaseDatabaseErrorEventArgs> Error;

        protected FirebaseDatabaseConnection(string path)
        {
            _ref = FirebaseDatabase.Instance.GetReference(path);
            _ref.AddChildEventListener(this);
        }


        protected void OnError(DatabaseError err)
        {
            Error?.Invoke(this, new FirebaseDatabaseErrorEventArgs(err));
        }
        protected abstract void OnCancelled(DatabaseException error);
        protected abstract void OnChildAdded(string id, T newChild);
        protected abstract void OnChildChanged(string id, T NewChild);
        protected abstract void OnChildMoved(string id, T NewChild);
        protected abstract void OnChildRemoved(string id, T removedChild);

        #region Implementations
        public void OnCancelled(DatabaseError error)
        {
            OnCancelled(error.ToException());
            OnError(error);
        }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {

            OnChildAdded(snapshot.Key,FirebaseHelper.SnapshotToObject<T>(snapshot));
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName)
        {
            OnChildChanged(snapshot.Key, FirebaseHelper.SnapshotToObject<T>(snapshot));
        }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName)
        {
            OnChildMoved(snapshot.Key, FirebaseHelper.SnapshotToObject<T>(snapshot));
        }

        public void OnChildRemoved(DataSnapshot snapshot)
        {
            OnChildRemoved(snapshot.Key, FirebaseHelper.SnapshotToObject<T>(snapshot));
        }
        #endregion

    }
}