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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent i;
            if (SharedPrefrenceManager.IsLoggedIn())
            {
                i = new Intent(this, typeof(MainPageActivity));

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