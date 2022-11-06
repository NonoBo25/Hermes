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
    public class MessagesAdapter : BaseAdapter<Message>
    {
        private Context sContext;
        private int chatId;
        public MessagesAdapter(Context context,int chatId)
        {
            sContext = context;
            this.chatId = chatId;
        }
        public override Message this[int position]
        {
            get
            {
                return App.ChatsManager.ChatList[chatId].Messages[position];
            }
        }
        public override int Count
        {
            get
            {
                return App.ChatsManager.ChatList[chatId].Messages.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            int res = Resource.Layout.cell_incoming_bubble;
            int mRes = Resource.Id.msg_text;
            int tRes = Resource.Id.msg_time;
            int imgRes = Resource.Id.msg_image;
            if (this[position].Sender.Equals(App.AuthManager.CurrentUserUid))
            {
                res = Resource.Layout.cell_outgoing_bubble;
            }
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(res, null, false);
                }

                TextView message = row.FindViewById<TextView>(mRes);
                message.Text = this[position].Content;
                TextView time = row.FindViewById<TextView>(tRes);
                time.Text = TextHelper.UnixToTime(this[position].Timestamp);
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