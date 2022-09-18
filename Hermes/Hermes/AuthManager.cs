using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    static class AuthManager
    {
        private static FirebaseAuth mAuth = FirebaseAuth.Instance;
        private static DatabaseReference mRef = FirebaseDatabase.Instance.GetReference("/users");
        public static bool SignUp(string email,string password,string username)
        {
            Task t = mAuth.CreateUserWithEmailAndPassword(email, password);
            if (t.IsSuccessful)
            {
                SharedPrefrenceManager.SaveUser(email, password);
                User u = new User(username);
                mRef.Child(mAuth.Uid).SetValue(u.ToHashMap());
                return true;
            }
            return false;
        }
        public static bool SignIn(string email, string password)
        {
            Task t = mAuth.SignInWithEmailAndPassword(email, password);
            if (t.IsSuccessful)
            {
                SharedPrefrenceManager.SaveUser(email, password);
                return true;
            }
            return false;
        }


    }
}