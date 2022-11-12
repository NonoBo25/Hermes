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
using AndroidX.Activity.Result;
using static AndroidX.Activity.Result.Contract.ActivityResultContracts;
using Java.IO;
using Android.Graphics;

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
        private FloatingActionButton mAttach;
        private Android.Net.Uri uri=null;
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
            mAttach = FindViewById<FloatingActionButton>(Resource.Id.image_attach);
            mAttach.Click += MAttach_Click;
        }

        private void MAttach_Click(object sender, EventArgs e)
        {
            PickVisualMediaRequest req = new PickVisualMediaRequest.Builder().SetMediaType(PickVisualMedia.ImageOnly.Instance).Build();
            PickVisualMedia p=new PickVisualMedia();
           
            StartActivityForResult(p.CreateIntent(this,req ), 120);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            switch (requestCode)
            {
                case 120:
                    if (resultCode == Result.Ok)
                    {
                        uri = data.Data;
                        Intent intent = new Intent(this,typeof(ImageMessageActivity));
                        intent.PutExtra("path",uri.ToString());
                        intent.PutExtra("chatId", chatId);
                        StartActivityForResult(intent, 220);

                    }
                    break;
                case 220:
                    if (resultCode == Result.Ok)
                    {
                        Toast.MakeText(this,"Message Sent!",ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Error Try Again!", ToastLength.Short).Show();
                    }
                    break;
                default:
                    break;

            }
        }

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
            if (!App.ChatsManager.SendMessage(m))
            {
                Toast.MakeText(this, "Error Sending Message!", ToastLength.Long).Show();
            }
            else
            {
                mMessage.Text = "";
            }
        }


    }
}