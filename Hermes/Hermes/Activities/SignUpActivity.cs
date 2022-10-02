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
using System.Text.RegularExpressions;

namespace Hermes
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        private Button signup;
        private EditText email,username,password1,password2;
        private FirebaseAuth mAuth;
        private DatabaseReference mReference;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mAuth = FirebaseAuth.Instance;
            mReference = FirebaseDatabase.Instance.GetReference("/users");
            
            SetContentView(Resource.Layout.activity_signup);
            signup = FindViewById<Button>(Resource.Id.signup_signup);
            email = FindViewById<EditText>(Resource.Id.signup_email);
            username = FindViewById<EditText>(Resource.Id.signup_username);
            password1 = FindViewById<EditText>(Resource.Id.signup_password1);
            password2 = FindViewById<EditText>(Resource.Id.signup_password2);
            signup.Click += Signup_Click;
        }
        private bool ArePasswordsEqual()
        {
            return password1.Text.Equals(password2.Text);
        }
        private void Signup_Click(object sender, EventArgs e)
        {
            if (ArePasswordsEqual())
            {
                UserData tempUser = new UserData { Email = email.Text, Password = password1.Text ,Username=username.Text};
                AuthHelper.UserVerificationResult res = AuthHelper.VerifyUserData(tempUser);
                if (res)
                {
                    if (App.AuthManager.SignUp(tempUser))
                    {
                        SharedPrefrenceManager.SaveUser(tempUser);
                        Intent i = new Intent(this, typeof(MainPageActivity));
                        i.PutExtra("StartForegroundService", true);
                        StartActivity(i);
                    }
                    else
                    {
                        Toast.MakeText(this, "Error Signing Up!\nTry Again!", ToastLength.Long).Show();
                        return;
                    }
                }
            }
        }
    }
}