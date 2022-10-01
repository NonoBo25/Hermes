using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public class ChatAdapter : BaseAdapter<Chat>
    {
        public List<Chat> sList;
        private Context sContext;
        private DatabaseReference mRef;
        public ChatAdapter(Context context, List<Chat> list)
        {
           

            sList = list;
            sContext = context;
            mRef = FirebaseDatabase.Instance.GetReference("/users");
        }
        public override Chat this[int position]
        {
            get
            {
                return sList[position];
            }
        }
        public override int Count
        {
            get
            {
                return sList.Count;
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
    }
}