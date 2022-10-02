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
    public class ChatAdapter : BaseAdapter<Chat>
    {
        private Context sContext;
        public ChatAdapter(Context context)
        {
            sContext = context;
            App.ChatsManager.PropertyChanged += OnNewMessages;
        }
        public override Chat this[int position]
        {
            get
            {
                return App.ChatsManager.ChatList[position];
            }
        }
        public override int Count
        {
            get
            {
                return App.ChatsManager.ChatList.Count;
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
                name.Text = App.UserManager.UsernameById[this[position].Partner];
                content.Text = this[position].Messages.Last().Content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }

        public void OnNewMessages(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(App.ChatsManager.ChatList))
            {
                NotifyDataSetChanged();
            }
        }
    }
}