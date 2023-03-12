using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using Java.IO;
using Java.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hermes
{
    public class MessagesAdapter : BaseAdapter<Message>
    {
        private Context sContext;
        private Dictionary<string,Bitmap> images;
        private ArrayList downloadedImages;
        private ArrayList chat;
        private MessagesConnection _connection;
        private string _partner;
        public MessagesAdapter(Context context,string partner)
        {
            sContext = context; 
            _partner = partner;
            downloadedImages=new ArrayList();
            images = new Dictionary<string,Bitmap>();
            chat = new ArrayList();
            _connection = new MessagesConnection(_partner);
            _connection.MessageAdded += OnNewMessage;
        }

        private void OnNewMessage(object sender, MessageEventArgs e)
        {
            //string partner = "";
            //if (e.Message.Type == MessageType.Incoming)
            //{
            //    partner = e.Message.Sender;
            //}
            //if (e.Message.Type == MessageType.Outgoing)
            //{
            //    partner = e.Message.Recipient;
            //}
            //if(partner != _partner)
            //{
            //    return;
            //}
            chat.Add(e.Message);
            NotifyDataSetChanged();
        }
        public Bitmap GetPosBitmap(int pos)
        {
            return images[((Message)chat[pos]).Timestamp];
        }

        public override Message this[int position]
        {
            get
            {
                return (Message)chat[position];
            }
        }
        public override int Count
        {
            get
            {
                return chat.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            Message m = this[position];
            int res = Resource.Layout.cell_incoming_bubble;
            if (m.Type ==MessageType.Outgoing)
            {
                res = Resource.Layout.cell_outgoing_bubble;
            }
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(res, null, false);

                }
                TextView message = row.FindViewById<TextView>(Resource.Id.msg_text);
                message.Text = m.Content;
                TextView time = row.FindViewById<TextView>(Resource.Id.msg_time);
                time.Text = TextHelper.UnixToTime(m.Timestamp);
                ImageView img = row.FindViewById<ImageView>(Resource.Id.msg_image);
                img.Visibility = ViewStates.Invisible;
                if (m.HasImage)
                {
                    Bitmap bmImg = BitmapFactory.DecodeResource(sContext.Resources, Resource.Drawable.forbidden);
                    if (m.Type== MessageType.Outgoing)
                    {
                        if (isImageDownloaded(m.Timestamp))
                        {
                            bmImg = images[m.Timestamp];
                        }
                        else
                        {
                            if (m.IsImageSafe)
                            {
                                try
                                {
                                    bmImg = BitmapFactory.DecodeStream(Application.Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(m.ImageUri)));
                                    images[m.Timestamp] = bmImg;
                                    downloadedImages.Add(m.Timestamp);
                                }
                                catch(Exception e) {
                                    Log.Error("ERROR",e.StackTrace);
                                }
                            }
                            images[m.Timestamp] = bmImg;
                            downloadedImages.Add(m.Timestamp);
                        }

                    }
                    else
                    {
                        if (isImageDownloaded(m.Timestamp))
                        {
                            bmImg = images[m.Timestamp];
                        }
                        else
                        {
                            bmImg = BitmapFactory.DecodeResource(sContext.Resources, Resource.Drawable.pending);
                            if (m.IsImageSafe)
                            {
                                Thread t = new Thread(new ThreadStart(delegate
                                {
                                    URL url = new URL(m.ImageLink);
                                    Bitmap bmp = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
                                    images[m.Timestamp] = bmp;
                                    downloadedImages.Add(m.Timestamp);
                                    ((Activity)sContext).RunOnUiThread(() =>
                                    {
                                        img.SetImageBitmap(images[m.Timestamp]);
                                        img.Visibility = ViewStates.Visible;
                                    });
                                }));
                                t.Start();
                            }

                        }
                    }
                    img.SetImageBitmap(images[m.Timestamp]);
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }



        private void Row_Click(object sender, EventArgs e)
        {
            
        }

        private bool isImageDownloaded(string timestamp)
        {
            return downloadedImages.Contains(timestamp);
        }
            
        
        public static int dpToPx(int dp, Context context)
        {
            float density = context.Resources.DisplayMetrics.Density;
            return (int)Math.Round((float)dp * density);
        }

    }
}