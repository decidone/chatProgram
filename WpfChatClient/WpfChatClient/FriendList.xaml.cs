using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfChatClient
{
    public partial class FriendList : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();

        public FriendList()
        {
            InitializeComponent();

            DataPacket dp = new DataPacket
            {
                work = "friend_list",
                user_id = MainWindow.userId
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            sel_friend_list.ItemsSource = Data.Current.FriendList;
        }

        private void Invite_friend_Click(object sender, RoutedEventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "invite_friend",
                room_num = MainWindow.chat_room,
                friend_id = this.sel_friend.Text
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/ChatRoom.xaml", UriKind.Relative);
        }
    }
}
