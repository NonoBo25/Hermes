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
    public class SignUpActivity : Activity,IOnCompleteListener
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

        private void Signup_Click(object sender, EventArgs e)
        {
            if ( !TextHelper.IsValidString(password1.Text)||!TextHelper.IsValidString(password2.Text)||!password1.Text.Equals(password2.Text))
            {
                Toast.MakeText(this, "Passwords Do Not Match!", ToastLength.Long).Show();
                return;
            }
            if (!TextHelper.IsValidString(email.Text)||!TextHelper.IsValidEmail(email.Text))
            {
                Toast.MakeText(this, "Invalid Email Adress!", ToastLength.Long).Show();
                return;
            }
            if (!TextHelper.IsValidString(username.Text))
            {
                Toast.MakeText(this, "Invalid Username!", ToastLength.Long).Show();
                return;
            }
            mAuth.CreateUserWithEmailAndPassword(email.Text, password1.Text).AddOnCompleteListener(this);

        }


        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                SharedPrefrenceManager.SaveUser(email.Text, password1.Text);
                mReference.Child(mAuth.CurrentUser.Uid).SetValue(new User(username.Text).ToHashMap());
                Intent i = new Intent(this, typeof(MainPageActivity));
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