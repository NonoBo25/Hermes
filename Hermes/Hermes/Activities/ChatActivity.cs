using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProfanityFilter;
using Google.Android.Material.FloatingActionButton;
using AndroidX.Activity.Result;
using Android.Provider;
using Firebase.Storage;
using Android.Gms.Tasks;
using AndroidX.DocumentFile.Provider;

namespace Hermes
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {
        private int chatId;
        private ListView mMessages;
        private TextView mUser;
        private EditText mMessage;
        private FloatingActionButton mSend;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chat);
            App.ChatsManager.PropertyChanged += ChatsManager_PropertyChanged;
            chatId = Intent.GetIntExtra("chatId", -1);
            mMessages = FindViewById<ListView>(Resource.Id.messages);
            mMessages.Adapter = new MessagesAdapter(this, chatId);
            mUser = FindViewById<TextView>(Resource.Id.chat_username);
            mUser.Text = App.UserManager.UsernameById[App.ChatsManager.ChatList[chatId].Partner];
            mMessage = FindViewById<EditText>(Resource.Id.message_input);
            
            mSend = FindViewById<FloatingActionButton>(Resource.Id.message_send);
            mSend.Click += MSend_Click;

            //Intent intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            //intent.SetType("image/* video/*");
            //StartActivityForResult(intent,120);
        }

        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (resultCode == Result.Ok)
        //    {
        //        StorageReference storageRef = FirebaseStorage.Instance.Reference;
        //        Android.Net.Uri d = data.Data;
        //        if (App.StorageManager.UploadFile(d))
        //        {
        //            Toast.MakeText(this, "SUccess", ToastLength.Long);
        //        }
                
        //    }
        //}

        private void ChatsManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            mMessages.Adapter = null;
            mMessages.Adapter=new MessagesAdapter(this, chatId);
        }

        private void MSend_Click(object sender, EventArgs e)
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            var censored = filter.CensorString(mMessage.Text);
            mMessage.Text = censored;
            Message m = new Message();
            m.Sender = App.AuthManager.CurrentUserUid;
            m.Recipient=App.ChatsManager.ChatList[chatId].Partner;
            m.Content = mMessage.Text;
            m.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            App.ChatsManager.SendMessage(m);
            mMessage.Text = "";
        }


    }
}