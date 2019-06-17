using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Chat
    {
        public string chat_id { get; set; }
        public string chat_message { get; set; }
        public DateTime chat_time { get; set; }

        public Chat(string chat_id, string chat_message, DateTime chat_time)
        {
            this.chat_id = chat_id;
            this.chat_message = chat_message;
            this.chat_time = chat_time;
        }
    }

    public class DataPacket
    {
        public string work { get; set; }
        public string user_id { get; set; }
        public string user_pw { get; set; }
        public string user_name { get; set; }
        public string message { get; set; }
        public string friend_id { get; set; }
        public int room_num { get; set; }
        //public string Email { get; set; }
        //public bool Active { get; set; }
        //public DateTime CreatedDate { get; set; }
        public IList<string> friend_list { get; set; }
        public IList<int> chat_list { get; set; }
        public IList<Chat> chat { get; set; }
    }
}
