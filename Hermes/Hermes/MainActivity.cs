using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Hermes
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button toSignIn, toSignUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                Intent i = new Intent(this, typeof(MainPageActivity));
                StartActivity(i);
            }
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            toSignIn = FindViewById<Button>(Resource.Id.mainactivity_to_signin);
            toSignUp =FindViewById<Button>(Resource.Id.mainactivity_to_signup);
            toSignUp.Click += delegate { Intent i = new Intent(this, typeof(SignUpActivity)); StartActivity(i); };
            toSignIn.Click += delegate { Intent i = new Intent(this, typeof(SignInActivity)); StartActivity(i); };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}