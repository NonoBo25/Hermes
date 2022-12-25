using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Firebase.Auth.FirebaseAuth;

namespace Hermes
{
    [Application]
    public class HermesApp:Application
    {

        protected HermesApp(System.IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        { }

        public override void OnCreate()
        {
            base.OnCreate();
        }
        public void StartService()
        {
        }


    }
}