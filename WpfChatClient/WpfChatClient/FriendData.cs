using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfChatClient
{
    class FriendData : INotifyPropertyChanged
    {

        public List<FriendData> DataSource = new List<FriendData>();

        private string friend;

        public string friend_id
        {
            get { return friend; }
            set {
                friend = value;
                OnPropertyChanged("friend_id");
            }
        }

        public FriendData()
        {
        }
        public FriendData(string friend)
        {
            this.friend_id = friend;
        }

        // Singleton
        public static FriendData Current = new FriendData();
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
