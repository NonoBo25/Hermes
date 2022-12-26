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
    [Activity(Label = "SignInActivity")]
    public class SignInActivity : Activity
    {
        private Button signin;
        private TextView email, password;
        private FirebaseAuth mAuth;
        
        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mAuth = FirebaseAuth.Instance;
            // Create your application here
            SetContentView(Resource.Layout.activity_signin);
            signin = FindViewById<Button>(Resource.Id.signin_signin);
            email = FindViewById<EditText>(Resource.Id.signin_email);
            password = FindViewById<TextView>(Resource.Id.signin_password);
            signin.Click += Signin_Click;
        }

        private void Signin_Click(object sender, EventArgs e)
        {
            UserData tempUser = new UserData { Email = email.Text, Password = password.Text };
            AuthHelper.UserVerificationResult res = AuthHelper.VerifyUserData(tempUser);
            if (res)
            {
                if (AuthManager.SignIn(tempUser))
                {
                    SharedPrefrenceManager.SaveUser(tempUser);
                    Intent i = new Intent(this, typeof(MainPageActivity));
                    i.PutExtra("StartForegroundService", true);
                    StartActivity(i);
                }
                else
                {
                    Toast.MakeText(this, "Error Signing In!\nTry Again!", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, res.ToString(), ToastLength.Long).Show();
            }
        }
        

        
    }
}