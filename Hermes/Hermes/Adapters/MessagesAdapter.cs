using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using Java.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hermes
{
    public class MessagesAdapter : BaseAdapter<Message>
    {
        private Context sContext;
        private int chatId;
        private Dictionary<string,Bitmap> images;
        public MessagesAdapter(Context context,int chatId)
        {
            sContext = context;
            this.chatId = chatId;
            images = new Dictionary<string,Bitmap>();
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
            if (this[position].Sender.Equals(App.AuthManager.CurrentUserUid))
            {
                res = Resource.Layout.cell_outgoing_bubble;
            }
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(res, null, false);
                    TextView message = row.FindViewById<TextView>(mRes);
                    message.Text = this[position].Content;
                    TextView time = row.FindViewById<TextView>(tRes);
                    time.Text = TextHelper.UnixToTime(this[position].Timestamp);
                    ImageView img = row.FindViewById<ImageView>(Resource.Id.msg_image);
                    img.Visibility = ViewStates.Invisible;
                    if (this[position].ImageUri != "" && this[position].ImageUri != null)
                    {
                        Bitmap bmImg = BitmapFactory.DecodeResource(sContext.Resources, Resource.Drawable.xamagonBlue);
                        if (this[position].Sender.Equals(App.AuthManager.CurrentUserUid))
                        {
                            if (this[position].IsImageSafe)
                            {
                                try
                                {
                                    bmImg = BitmapFactory.DecodeStream(Application.Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(this[position].ImageUri)));

                                }
                                catch { }


                            }
                            img.SetImageBitmap(bmImg);
                            img.Visibility = ViewStates.Visible;

                        }
                        else
                        {
                            if (App.ChatsManager.ChatList[chatId].Images.ContainsKey(this[position].Timestamp))
                            {
                                bmImg = App.ChatsManager.ChatList[chatId].Images[this[position].Timestamp];
                            }
                            else if (this[position].ImageLink != "" && this[position].ImageLink != null)
                            {
                                if (this[position].IsImageSafe)
                                {
                                    Thread t = new Thread(new ThreadStart(delegate
                                    {
                                        URL url = new URL(this[position].ImageLink);
                                        Bitmap bmp = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
                                        App.ChatsManager.ChatList[chatId].Images[this[position].Timestamp] = bmp;
                                        ((Activity)sContext).RunOnUiThread(() =>
                                        {
                                            img.SetImageBitmap(bmp);
                                            img.Visibility = ViewStates.Visible;
                                        });
                                    }));
                                    t.Start();
                                }

                            }
                        }
                        img.SetImageBitmap(bmImg);
                        img.Visibility = ViewStates.Visible;
                        img.LayoutParameters.Width = dpToPx(200, sContext);
                        img.LayoutParameters.Height = dpToPx(200, sContext);
                    }
                    else
                    {
                        img.LayoutParameters.Width = 0;
                        img.LayoutParameters.Height = 0;
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }
        public static int dpToPx(int dp, Context context)
        {
            float density = context.Resources.DisplayMetrics.Density;
            return (int)Math.Round((float)dp * density);
        }

    }
}