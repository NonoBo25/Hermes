using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    [Activity(Label = "ImageMessageActivity")]
    public class ImageMessageActivity : Activity
    {
        private Android.Net.Uri imgUri;
        private ImageView imageView;
        private EditText message;
        private FloatingActionButton btn;
        private int chatId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_image_message);
            imageView = FindViewById<ImageView>(Resource.Id.img);
            message= FindViewById<EditText>(Resource.Id.message_input);
            btn = FindViewById<FloatingActionButton>(Resource.Id.message_send);
            btn.Click += Btn_Click;
            imgUri = Android.Net.Uri.Parse(Intent.GetStringExtra("path"));
            chatId = Intent.GetIntExtra("chatId", -1);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            var censored = filter.CensorString(message.Text);
            message.Text = censored;
            Message m = new Message();
            m.Sender = App.AuthManager.CurrentUserUid;
            m.Recipient = App.ChatsManager.ChatList[chatId].Partner;
            m.Content = message.Text;
            m.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            m.AttachImage(imgUri);
            if (!App.ChatsManager.SendMessage(m))
            {
                Toast.MakeText(this, "Error Sending Message!", ToastLength.Long).Show();
            }
            else
            {
                this.SetResult(Result.Ok);
                Finish();
            }
            
        }

        protected override void OnStart()
        {
            base.OnStart();
            Bitmap bmImg = BitmapFactory.DecodeStream(ContentResolver.OpenInputStream(imgUri));
            imageView.SetImageBitmap(bmImg);
        }
    }
}