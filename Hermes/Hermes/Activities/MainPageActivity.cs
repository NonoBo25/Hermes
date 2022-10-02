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
using System.ComponentModel;
using System.Linq;
using System.Text;
namespace Hermes
{


    [Activity(Label = "MainPageActivity", Enabled = true,Exported =true)]
    public class MainPageActivity : Activity
    {
        private ListView mListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            App.ChatsManager.Start();

            Log.Info("Main", "MainActivity");
            SetContentView(Resource.Layout.activity_mainpage);
            mListView = FindViewById<ListView>(Resource.Id.Chats);
            mListView.Adapter = new ChatAdapter(this);
            mListView.ItemClick += MListView_ItemClick;
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Message m = new Message { Content = "Hi", Sender = FirebaseAuth.Instance.CurrentUser.Uid, Recipient = App.UserManager.IdByUsername[this[position].Partner] };
             App.ChatsManager.SendMessage(m);
          
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (Intent.GetBooleanExtra("StartForegroundService", false))
            {
                SharedPrefrenceManager.StartForegroundService();
                Intent service = new Intent(this, typeof(CommunicationService));
                StartForegroundService(service);
            }
        }
    }
}