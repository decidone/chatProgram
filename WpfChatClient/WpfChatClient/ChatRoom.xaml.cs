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
    /// <summary>
    /// ChatRoom.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoom : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();

        public ChatRoom()
        {
            InitializeComponent();
        }

        private void Invite_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/FriendList.xaml", UriKind.Relative);
        }
    }
}
