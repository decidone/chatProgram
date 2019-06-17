using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatClient
{
    class ChatMessage : INotifyPropertyChanged
    {
        public List<ChatMessage> Chat = new List<ChatMessage>();

        private string _chat_id;

        public string chat_id
        {
            get { return _chat_id; }
            set {
                _chat_id = value;
                OnPropertyChanged("chat_id");
            }
        }
        private string _chat_message;

        public string chat_message
        {
            get { return _chat_message; }
            set {
                _chat_message = value;
                OnPropertyChanged("chat_message");
            }
        }

        private DateTime _chat_time;

        public DateTime chat_time
        {
            get { return _chat_time; }
            set {
                _chat_time = value;
                OnPropertyChanged("chat_time");
            }
        }

        private int _not_read;

        public int not_read
        {
            get { return _not_read; }
            set { _not_read = value; }
        }

        public ChatMessage()
        {

        }

        public ChatMessage(string chat_id, string chat_message, DateTime chat_time, int not_read)
        {
            this.chat_id = chat_id;
            this.chat_message = chat_message;
            this.chat_time = chat_time;
            this.not_read = not_read;
        }

        public static ChatMessage Current = new ChatMessage();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
