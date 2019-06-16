using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfChatClient
{
    class Data : INotifyPropertyChanged
    {

        public List<Data> FriendList = new List<Data>();
        public List<Data> ChatList = new List<Data>();

        private string friend;

        public string friend_id
        {
            get { return friend; }
            set {
                friend = value;
                OnPropertyChanged("friend_id");
            }
        }

        private int chat;

        public int chat_room
        {
            get { return chat; }
            set {
                chat = value;
                OnPropertyChanged("chat_room");
            }
        }


        public Data() { }
        public Data(string friend)
        {
            this.friend_id = friend;
        }
        public Data(int chat_room)
        {
            this.chat_room = chat_room;
        }

        // Singleton
        public static Data Current = new Data();
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
