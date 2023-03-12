using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class ChatAdapter : BaseAdapter<ChatSimplified>
    {
        private Context sContext;
        private List<ChatSimplified> mSimplifiedList;
        
        public ChatAdapter(Context context,List<ChatSimplified> l)
        {
            sContext = context;
            mSimplifiedList = l.ToList();
        }



        public override ChatSimplified this[int position]
        {
            get
            {
                return mSimplifiedList[position];
            }
        }
        public override int Count
        {
            get
            {
                return mSimplifiedList.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.cell_chat, null, false);
                }
                
                TextView name = row.FindViewById<TextView>(Resource.Id.chat_name);
                TextView content = row.FindViewById<TextView>(Resource.Id.chat_content);
                TextView time = row.FindViewById<TextView>(Resource.Id.chat_time);
                name.Text = UserManager.GetUsername(this[position].Partner);
                content.Text = this[position].LatestMessageContent;
                time.Text = TextHelper.UnixToTime(this[position].LatestMessageTimestamp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }

        
    }
}