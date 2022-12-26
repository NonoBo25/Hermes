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
    [Activity(Label = "ImageViewerActivity")]
    public class ImageViewerActivity : Activity
    {
        private Bitmap _bitmap;
        private ImageView _imageView;
        private FloatingActionButton _download;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_image_viewer);
            string path = Intent.GetStringExtra("bitmap");
            _bitmap = BitmapFactory.DecodeFile(path);
            _imageView = FindViewById<ImageView>(Resource.Id.img);
            _imageView.SetImageBitmap(_bitmap);
            
            _download = FindViewById<FloatingActionButton>(Resource.Id.download);
            _download.Click += Download;
        }



        private void Download(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string name = dt.Ticks.ToString();
            Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string path = System.IO.Path.Combine(storagePath.ToString(), name) + ".jpg";
            MediaHelper.DownloadBitmap(path, _bitmap, this); 
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
            SendBroadcast(mediaScanIntent);
        }
    }
}