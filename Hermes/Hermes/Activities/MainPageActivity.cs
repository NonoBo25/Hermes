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
        private SearchView search;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            App.ChatsManager.Start();
            App.ChatsManager.PropertyChanged += ChatsManager_PropertyChanged;
            Log.Info("Main", "MainActivity");
            SetContentView(Resource.Layout.activity_mainpage);
            mListView = FindViewById<ListView>(Resource.Id.Chats);
            mListView.Adapter = new ChatAdapter(this);
            mListView.ItemClick += MListView_ItemClick;
            search = FindViewById<SearchView>(Resource.Id.search);
            search.QueryTextSubmit += Search_QueryTextSubmit;
        }

        private void ChatsManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                ((ChatAdapter)mListView.Adapter).NotifyDataSetChanged();
            });
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("chatId", e.Position);
            StartActivity(i);
        }

        private void Search_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (App.UserManager.Exists(e.Query))
            {
                if (App.ChatsManager.ChatExists(App.UserManager.IdByUsername[e.Query]) == -1)
                {
 
                    App.ChatsManager.NewChat(App.UserManager.IdByUsername[e.Query]);
                }
            }
            else
            {
                Toast.MakeText(this, "User Not Found!", ToastLength.Long).Show();
                return;
            }

            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("chatId", App.ChatsManager.ChatExists(App.UserManager.IdByUsername[e.Query]));
            StartActivity(i);
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