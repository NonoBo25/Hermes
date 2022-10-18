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
using System.Text.RegularExpressions;

namespace Hermes
{
    public static class TextHelper
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            var regex = new Regex(pattern, RegexOptions.None);
            return regex.IsMatch(email);
        }
        public static bool IsValidString(string str)
        {
            if (str != null)
            {
                if (str.Length != 0)
                {
                    if (str != "")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static string UnixToTime(string unix)
        {
            long sec = (long)double.Parse(unix);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(sec).ToLocalTime();
            return $"{dateTime.Hour}:{dateTime.Minute}";
        }
    }
}