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
        private string partner;
        private ListView mMessages;
        private TextView mUser;
        private EditText mMessage;
        private FloatingActionButton mSend;
        private FloatingActionButton mAttach;
        private Android.Net.Uri uri=null;
        private ChatModel model;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_chat);
            //App.ChatsManager.PropertyChanged += ChatsManager_PropertyChanged;
            partner = Intent.GetStringExtra("partner");
            model = new ChatModel(partner);
            
            mMessages = FindViewById<ListView>(Resource.Id.messages);
            mMessages.Adapter = new MessagesAdapter(this);
            mUser = FindViewById<TextView>(Resource.Id.chat_username);
            mUser.Text = UserManager.GetUsername(partner);
            mMessage = FindViewById<EditText>(Resource.Id.message_input);
            mSend = FindViewById<FloatingActionButton>(Resource.Id.message_send);
            mSend.Click += MSend_Click;
            mAttach = FindViewById<FloatingActionButton>(Resource.Id.image_attach);
            mAttach.Click += MAttach_Click;
            mMessages.ScrollingCacheEnabled = true;

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
                        Application.Context.ContentResolver.TakePersistableUriPermission(uri, ActivityFlags.GrantReadUriPermission) ;
                        Intent intent = new Intent(this,typeof(ImageMessageActivity));
                        intent.PutExtra("path",uri.ToString());
                        intent.PutExtra("partner", partner);
                        StartActivityForResult(intent, 220);

                    }
                    break;
                default:
                    break;

            }
        }
        private void MSend_Click(object sender, EventArgs e)
        {
            mMessage.Text = TextHelper.CensorText(mMessage.Text);
            model.SendMessage(mMessage.Text);
        }


    }
}