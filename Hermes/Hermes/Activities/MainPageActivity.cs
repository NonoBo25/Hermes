using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
    
    [Activity(Label = "MainPageActivity", Enabled = true,Exported =true)]
    public class MainPageActivity : Activity
    {
        private DatabaseReference mRef;
        private FirebaseAuth mAuth;
        private List<Message> mMessages;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Info("Main", "MainActivity");
            SetContentView(Resource.Layout.activity_mainpage);
            mAuth = FirebaseAuth.Instance;
            mRef = FirebaseDatabase.Instance.GetReference("/inboxes").Child(FirebaseAuth.Instance.CurrentUser.Uid);
            mMessages = new List<Message>();
            mRef.AddValueEventListener(new InboxListener(this));
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (Intent.GetBooleanExtra("StartService", false))
            {
                SharedPrefrenceManager.StartService();
                Intent service = new Intent(this, typeof(CommunicationService));
                StartForegroundService(service);
            }
        }

        class InboxListener : Java.Lang.Object, IValueEventListener
        {
            private MainPageActivity obj;
            public InboxListener(MainPageActivity obj)
            {
                this.obj = obj;
            }

            public void OnCancelled(DatabaseError error)
            {
                throw new NotImplementedException();
            }

            public void OnDataChange(DataSnapshot snapshot)
            {
                foreach (DataSnapshot i in snapshot.Children.ToEnumerable())
                {
                    i.Value.JavaCast<JavaDictionary>();
                }

                
                
            }
        }
    }
}