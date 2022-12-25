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
using System.Threading;

namespace Hermes
{

    [Activity(Label = "MainPageActivity", Enabled = true, Exported = true)]
    public class MainPageActivity : Activity
    {
        private ListView mListView;
        private SearchView search;
        private MainPageModel model;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SharedPrefrenceManager.StartForegroundService();
            Intent i = new Intent(this, typeof(CommunicationService));
            StartForegroundService(i);
            model = new MainPageModel();
            model.DataChanged += OnNewMessage;
            SetContentView(Resource.Layout.activity_mainpage);
            mListView = FindViewById<ListView>(Resource.Id.Chats);
            mListView.Adapter = new ChatAdapter(this,new List<ChatSimplified>());
            mListView.ItemClick += MListView_ItemClick;
            search = FindViewById<SearchView>(Resource.Id.search);
            search.QueryTextSubmit += Search_QueryTextSubmit;

        }

        private void OnNewMessage(object sender, MainPageEventArgs e)
        {
            RunOnUiThread(() =>
            {
                mListView.Adapter = new ChatAdapter(this, e.ChatList);
            });
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("partner", model.GetPartner(e.Position));
            
            StartActivity(i);
        }

        private void Search_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (UserManager.GetUid(e.Query)==null)
            {
                Toast.MakeText(this, "User Not Found!", ToastLength.Long).Show();
                return;
            }

            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("partner", UserManager.GetUid(e.Query));
            StartActivity(i);
        }

    }

}