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
    public static class UserManager
    {
        private static DatabaseReference mRef = FirebaseDatabase.Instance.GetReference("/users");
        public static bool RegisterUsername(string Username)
        {
            Task registerUsername = mRef.Child(AuthManager.CurrentUserUid).SetValue(Username);
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate
                {
                    Android.Gms.Tasks.TasksClass.Await(registerUsername);
                    return;
                }));
                thr.Start();
                thr.Join();
                return registerUsername.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }
        public static string GetUsername(string uid)
        {
            Task getUsername = mRef.Child(uid).Get();
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate
                {
                    Android.Gms.Tasks.TasksClass.Await(getUsername);
                    return;
                }));
                thr.Start();
                thr.Join();
                try
                {

                    return ((DataSnapshot)getUsername.Result).Value.ToString();
                }
                catch
                {
                    return null ;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string GetUid(string username)
        {
            Task GetUid = mRef.OrderByValue().EqualTo(username).Get() ;
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate
                {
                    Android.Gms.Tasks.TasksClass.Await(GetUid);
                    return;
                }));
                thr.Start();
                thr.Join();
                try
                {
                    DataSnapshot snapshot= (DataSnapshot)GetUid.Result;
                    Java.Util.HashMap m = (snapshot.Value.JavaCast<Java.Util.HashMap>());
                    List<string> keys = new List<string>();
                    foreach(string key in m.KeySet())
                    {
                        keys.Add(key);
                    }
                    return keys[0];
                }
                catch
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
    }
}