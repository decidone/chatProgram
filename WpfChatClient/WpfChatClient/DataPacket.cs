using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatClient
{
    class DataPacket
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
    }
}
