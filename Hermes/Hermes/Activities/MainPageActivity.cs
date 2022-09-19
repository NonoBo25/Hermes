﻿using Android.App;
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
    [Activity(Label = "MainPageActivity")]
    public class MainPageActivity : Activity
    {
        LinearLayout layout;
        DatabaseReference mRef;
        FirebaseAuth mAuth;
        CommunicationServiceConnection serviceConnection;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_mainpage);
            layout = FindViewById<LinearLayout>(Resource.Id.bg);
            mAuth = FirebaseAuth.Instance;
            mRef = FirebaseDatabase.Instance.GetReference("/inboxes");
            //mRef.AddValueEventListener(new InboxListener(this));
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (serviceConnection == null)
            {
                this.serviceConnection = new CommunicationServiceConnection();
            }
            Intent serviceToStart = new Intent(this,typeof(CommunicationService));
            BindService(serviceToStart, this.serviceConnection, Bind.AutoCreate);
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
                throw new NotImplementedException();
            }
        }
    }
}