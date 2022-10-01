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
    public class ChatsManager
    {
        private Dictionary<string, Chat> _chats;

        public ChatsManager()
        {
            _chats = new Dictionary<string, Chat>();
        }
        public ChatsManager(List<Message> l)
        {
            _chats = new Dictionary<string, Chat>();
            foreach(Message i in l)
            {
                AddMessage(i);
            }
        }

        public List<Chat> GetListOfChats()
        {
            return _chats.Values.ToList();
        }
        
        public void AddMessage(Message m)
        {
            if (_chats.Keys.Contains(m.Sender))
            {
                _chats[m.Sender].Messages.Add(m);
            }
            else
            {
                _chats[m.Sender]=new Chat(m.Sender,new List<Message> { m });
            }
        }
    }
}