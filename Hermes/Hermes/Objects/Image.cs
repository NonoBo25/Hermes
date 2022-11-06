using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class Image : IFirebaseObject
    {
        public string Name { set; get; }
        public bool IsSafe { get; set; }
        public void FromHashMap(HashMap map)
        {
            Name = map.Get("name").ToString();
            IsSafe = bool.Parse( map.Get("issafe").ToString());
        }

        public HashMap ToHashMap()
        {
            HashMap map = new HashMap();
            map.Put("name", Name);
            map.Put("issafe", IsSafe);
            return map;
        }
    }
}