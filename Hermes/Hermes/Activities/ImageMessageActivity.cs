using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string partner;
        private ImageMessageModel model;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_image_message);
            imageView = FindViewById<ImageView>(Resource.Id.img);
            message= FindViewById<EditText>(Resource.Id.message_input);
            btn = FindViewById<FloatingActionButton>(Resource.Id.message_send);
            btn.Click += Btn_Click;
            imgUri = Android.Net.Uri.Parse(Intent.GetStringExtra("path"));
            partner = Intent.GetStringExtra("partner");
            model = new ImageMessageModel(partner, imgUri);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            message.Text = TextHelper.CensorText(message.Text);
            model.SendMessage(message.Text);
            Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Bitmap bmImg = BitmapFactory.DecodeStream(ContentResolver.OpenInputStream(imgUri));
            imageView.SetImageBitmap(bmImg);
        }

    }
}