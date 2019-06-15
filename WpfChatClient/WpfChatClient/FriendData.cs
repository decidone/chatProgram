using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatClient
{
    class FriendData : Notifier
    {

        public static ObservableCollection<FriendData> DataSource = new ObservableCollection<FriendData>();

        private string friend;

        public string friend_id
        {
            get { return friend; }
            set {
                friend = value;
                OnPropertyChanged("friend_id");
            }
        }

        public FriendData(string friend)
        {
            this.friend_id = friend;
        }
    }
}
