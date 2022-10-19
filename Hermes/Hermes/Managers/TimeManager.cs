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
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading;

namespace Hermes
{
    public class TimeManager
    {
        private string api = "https://currentmillis.com/time/milliseconds-since-unix-epoch.php";
        private double _ts;
        public TimeManager() {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                 _ts=double.Parse(reader.ReadToEnd(), CultureInfo.InvariantCulture);
            }
            Thread thr = new Thread(new ThreadStart(add));
            thr.Start();

        }
        private void add()
        {
            while (true)
            {
                _ts += 10;
                Thread.Sleep(10);
            }

        }

        public string Timestamp { get=>_ts.ToString(); }
    }
}