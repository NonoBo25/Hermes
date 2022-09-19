using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    internal class User : IFirebaseObject
    {

        private string _username;

        public User(string username)
        {
            _username = username;
        }

        public HashMap ToHashMap()
        {
            HashMap result= new HashMap();
            result.Put("username", _username);
            return result;
        }
    }
}