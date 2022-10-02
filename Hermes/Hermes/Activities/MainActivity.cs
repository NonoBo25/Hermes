using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using Com.Judemanutd.Autostarter;
using Firebase.Auth;
using Firebase.Iid;

namespace Hermes
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true,Enabled =true,Exported =true, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : AppCompatActivity
    {
        FirebaseAuth mAuth;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (SharedPrefrenceManager.IsFirstTime())
            {
                SharedPrefrenceManager.FirstTime();
            

            }
            mAuth = FirebaseAuth.Instance;
            Intent i = new Intent(this, typeof(MainPageActivity));
            i.PutExtra("StartForegroundService", true);
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                if (mAuth.CurrentUser == null)
                {
                    UserData loggedUser = SharedPrefrenceManager.GetLoggedUser();
                    if (!App.AuthManager.SignIn(loggedUser))
                    {
                        Toast.MakeText(this, "Could Not Log In!", ToastLength.Long).Show();
                        i = new Intent(this, typeof(AuthMenuActivity));
                    }                  
                }
            }
            else
            {
                i = new Intent(this, typeof(AuthMenuActivity));
            }
            i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask |ActivityFlags.ClearTop);
            StartActivity(i);
            Finish();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}