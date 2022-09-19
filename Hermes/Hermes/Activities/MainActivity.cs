using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;

namespace Hermes
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity,IOnCompleteListener
    {
        FirebaseAuth mAuth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mAuth = FirebaseAuth.Instance;
            Intent i; 
            Intent serviceToStart = new Intent(this, typeof(CommunicationService));
            StartService(serviceToStart);
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                if (mAuth.CurrentUser == null)
                {
                    UserData loggedUser = SharedPrefrenceManager.GetLoggedUser();
                    mAuth.SignInWithEmailAndPassword(loggedUser.Email, loggedUser.Password).AddOnCompleteListener(this);
                    return;
                }
                else
                {
                    i = new Intent(this, typeof(MainPageActivity));
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

        public void OnComplete(Task task)
        {
            Intent i;
            if (task.IsSuccessful)
            {
                i=new Intent(this, typeof(MainPageActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask | ActivityFlags.ClearTop);
                StartActivity(i);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Could Not Log In!", ToastLength.Long).Show();
                i = new Intent(this, typeof(AuthMenuActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask | ActivityFlags.ClearTop);
                StartActivity(i);
                Finish();
            }
        }
    }
}