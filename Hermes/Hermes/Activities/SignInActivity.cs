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
        private DatabaseReference mReference;
        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mAuth = FirebaseAuth.Instance;
            mReference = FirebaseDatabase.Instance.GetReference("/users");
            // Create your application here
            SetContentView(Resource.Layout.activity_signin);
            signin = FindViewById<Button>(Resource.Id.signin_signin);
            email = FindViewById<EditText>(Resource.Id.signin_email);
            password = FindViewById<TextView>(Resource.Id.signin_password);
            signin.Click += Signin_Click;
        }

        private void Signin_Click(object sender, EventArgs e)
        {
            if (TextHelper.IsValidString(email.Text) && TextHelper.IsValidEmail(email.Text))
            {
                if (TextHelper.IsValidString(password.Text))
                {
                    mAuth.SignInWithEmailAndPassword(email.Text, password.Text).AddOnCompleteListener(new SignInListener(this));
                }
                else
                {
                    Toast.MakeText(this, "Invalid Password!", ToastLength.Long).Show();
                    return;
                }

            }
            else
            {
                Toast.MakeText(this, "Invalid Email Adress!", ToastLength.Long).Show();
                return;
            }
        }

        class SignInListener : Java.Lang.Object, IOnCompleteListener
        {
            private SignInActivity obj;
            public SignInListener(SignInActivity context)
            {
                obj = context;
            }
            public void OnComplete(Task task)
            {
                if (task.IsSuccessful)
                {
                    SharedPrefrenceManager.SaveUser(obj.email.Text, obj.password.Text);
                    Intent i = new Intent(obj, typeof(MainPageActivity));
                    i.PutExtra("StartForegroundService", true);
                    obj.StartActivity(i);
                }
                else
                {
                    Toast.MakeText(obj, "Error Signing Up!\nTry Again!", ToastLength.Long).Show();
                    return;
                }
            }
        }
    }
}