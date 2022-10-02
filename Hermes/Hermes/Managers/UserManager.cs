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

namespace Hermes
{
    public class UserManager : Java.Lang.Object,IValueEventListener
    {
        private DatabaseReference mRef;
        private Dictionary<string, string> usernameById;
        private Dictionary<string, string> idByUsername;


        public UserManager()
        {
            mRef = FirebaseDatabase.Instance.GetReference("/users");
            mRef.AddValueEventListener(this);
            usernameById = new Dictionary<string, string>();
            idByUsername = new Dictionary<string, string>();
        }

        public Dictionary<string, string> UsernameById { get => usernameById; }
        public Dictionary<string, string> IdByUsername { get => idByUsername; }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }


        public void OnDataChange(DataSnapshot snapshot)
        {
            foreach (DataSnapshot i in snapshot.Children.ToEnumerable())
            {
                usernameById[i.Key.ToString()] = i.Value.ToString();
                idByUsername[i.Value.ToString()] = i.Key.ToString();
            }
        }

        public void RegisterUsername(string uid,string username)
        {
            mRef.Child(uid).SetValue(new Java.Lang.String(username));
        }

    }
}