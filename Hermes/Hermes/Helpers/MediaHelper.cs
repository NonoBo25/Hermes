using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes
{
    public static class MediaHelper
    {
        public static bool isSafe(Android.Net.Uri image)
        {
            TensorflowClassifier cls = new TensorflowClassifier();
            float[] res= cls.Classify(UriToByteArray(image));
            return res[1] < 0.015;
        }
        private static byte[] UriToByteArray(Android.Net.Uri uri)
        {
            Bitmap bm = BitmapFactory.DecodeStream(Application.Context.ContentResolver.OpenInputStream(uri)); ;
            MemoryStream s = new MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Jpeg, 100, s); //bm is the bitmap object
            return s.ToArray();
        }
        public static void DownloadBitmap(string path,Bitmap bmp,Context context)
        {
            MemoryStream baos = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, baos);
            byte[] imageInByte = baos.ToArray();
            System.IO.File.WriteAllBytes(path, imageInByte);
        }
        public static Bitmap ImageViewToBitmap(ImageView imgv)
        {
            return ((BitmapDrawable)imgv.Drawable).Bitmap; 
        }
        
    }
}