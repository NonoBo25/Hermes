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
    [Activity(Label = "AuthMenuActivity")]
    public class AuthMenuActivity : Activity
    {
        private Button toSignIn, toSignUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_authmenu);
            toSignIn = FindViewById<Button>(Resource.Id.authmenu_to_signin);
            toSignUp = FindViewById<Button>(Resource.Id.authmenu_to_signup);
            toSignUp.Click += delegate { Intent i = new Intent(this, typeof(SignUpActivity)); StartActivity(i); };
            toSignIn.Click += delegate { Intent i = new Intent(this, typeof(SignInActivity)); StartActivity(i); };
        }
    }
}