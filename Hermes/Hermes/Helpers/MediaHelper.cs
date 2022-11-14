using Android.App;
using Android.Content;
using Android.Graphics;
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

namespace Hermes
{
    public static class MediaHelper
    {
        public static bool isSafe(Android.Net.Uri image)
        {
            TensorflowClassifier cls = new TensorflowClassifier();
            float[] res=cls.Classify(UriToByteArray(image));
            return res[0] > res[1];
        }
        private static byte[] UriToByteArray(Android.Net.Uri uri)
        {
            Bitmap bm = BitmapFactory.DecodeStream(Application.Context.ContentResolver.OpenInputStream(uri)); ;
            MemoryStream s = new MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Jpeg, 100, s); //bm is the bitmap object
            return s.ToArray();

        }
    }
}