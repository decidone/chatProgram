using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Friend.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Friend : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();

        public Friend()
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
            
            friend_list.ItemsSource = FriendData.DataSource;
        }

        private void Add_friend_Click(object sender, RoutedEventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "add_friend",
                user_id = MainWindow.userId,
                friend_id = this.new_friend.Text
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
    }
}
