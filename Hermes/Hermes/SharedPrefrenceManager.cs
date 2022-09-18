using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    static class SharedPrefrenceManager
    {
        public static void SaveUser(string email,string password)
        {
            ISharedPreferencesEditor mEdit= Application.Context.GetSharedPreferences("Users", FileCreationMode.Private).Edit();
            mEdit.PutString("email", email);
            mEdit.PutString("password", password);
            mEdit.Apply();
        }

        public static bool IsLoggedIn()
        {
            ISharedPreferences mPref = Application.Context.GetSharedPreferences("Users", FileCreationMode.Private);
            string email = mPref.GetString("email", null);
            string password = mPref.GetString("password", null);
            return !(email.Equals(null) || password.Equals(null));
        }


    }
}