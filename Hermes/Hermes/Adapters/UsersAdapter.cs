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

namespace Hermes
{
    public class UsersAdapter : BaseAdapter<string>
    {
        private Context _context;
        private string[] _users;
        
        public UsersAdapter(Context context,string[] users)
        {
            _context = context;
            _users = users;
        }
        public override string this[int position]
        {
            get
            {
                return _users[position];
            }
        }

        public override int Count
        {
            get
            {
                return _users.Length;
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
                    row = LayoutInflater.From(_context).Inflate(Resource.Layout.cell_username, null, false);
                }
                TextView tv = row.FindViewById<TextView>(Resource.Id.user_name);
                tv.Text = this[position];
            }
            catch { }
            finally { }
            return row;
        }
    }
}