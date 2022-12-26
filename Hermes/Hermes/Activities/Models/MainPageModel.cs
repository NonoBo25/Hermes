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
    public class ChatSimplified
    {
        public string Partner { get; set; }
        public string LatestMessageContent { get; set; }
        public string LatestMessageTimestamp { get; set; }
    }
    public class MainPageEventArgs : EventArgs
    {
        private List<ChatSimplified> _chatList;
        public List<ChatSimplified> ChatList 
        { 
            get
            {
               return _chatList.OrderBy(x => long.Parse(x.LatestMessageTimestamp)).ToList();
            }
            
        }
        public MainPageEventArgs(List<ChatSimplified> l)
        {
            _chatList = l.ToList();
        }
    }
    public class MainPageModel
    {
        public event EventHandler<MainPageEventArgs> DataChanged;
        MessagesConnection _connection;
        Dictionary<string,Message> _messages;
        public MainPageModel() { 
            _connection = new MessagesConnection();
            _connection.MessageAdded += OnNewMessage;
            _messages = new Dictionary<string, Message>();
        }
        private void OnDataChange()
        {
            List<ChatSimplified> chats=new List<ChatSimplified> ();
            foreach(string key in _messages.Keys)
            {
                chats.Add(new ChatSimplified() { LatestMessageContent = _messages[key].Content, LatestMessageTimestamp = _messages[key].Timestamp, Partner = key });
            }
            DataChanged?.Invoke(this, new MainPageEventArgs(chats));
        }

        public string GetPartner(int position)
        {
            return _messages.Keys.ToList()[position];
        }

        private void OnNewMessage(object sender, MessageEventArgs e)
        {
            string partner = e.Message.Sender;
            if (e.Message.Type == MessageType.Outgoing)
            {
                partner = e.Message.Recipient;
            }
            _messages[partner] = e.Message;
            OnDataChange();
        }

        public ChatSimplified DoesChatExist(string partnerId)
        {
            if (_messages.Keys.Contains(partnerId))
            {
                return new ChatSimplified() { LatestMessageContent = _messages[partnerId].Content, LatestMessageTimestamp = _messages[partnerId].Timestamp, Partner = partnerId };
            }
            return null;
        }
        
    }
}