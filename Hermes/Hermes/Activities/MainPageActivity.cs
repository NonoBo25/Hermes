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
        private ListView mListViewChats;
        private ListView mListViewSearch;
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
            mListViewChats = FindViewById<ListView>(Resource.Id.Chats);
            mListViewChats.Adapter = new ChatAdapter(this,new List<ChatSimplified>());
            mListViewChats.ItemClick += MListView_ItemClick;
            search = FindViewById<SearchView>(Resource.Id.search);
            search.QueryTextChange += SearchQueryChanged;
            mListViewSearch = FindViewById<ListView>(Resource.Id.search_res);
            mListViewSearch.ItemClick += OnSearchResultClick;

        }

        private void OnSearchResultClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string username = ((UsersAdapter)((ListView)sender).Adapter)[e.Position];
            string uid = model.GetUidFromUsername(username);
            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("partner", uid);
            StartActivity(i);
        }

        private void SearchQueryChanged(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if(e.NewText == "" || e.NewText == null)
            {
                mListViewSearch.Adapter = new UsersAdapter(this, new string[0]);
                return;
            }
            string[] res = model.SearchUser(e.NewText);
            mListViewSearch.Adapter = new UsersAdapter(this, res);
        }

        private void OnNewMessage(object sender, MainPageEventArgs e)
        {
            RunOnUiThread(() =>
            {
                mListViewChats.Adapter = new ChatAdapter(this, e.ChatList);
            });
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(ChatActivity));
            i.PutExtra("partner", model.GetPartner(e.Position));
            
            StartActivity(i);
        }


    }

}