using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Menu.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Menu : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();

        public Menu()
        {
            InitializeComponent();

            chat_room_list.ItemsSource = Data.Current.ChatList;
        }

        private void Newchat_Click(object sender, RoutedEventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "new_chat",
                user_id = MainWindow.userId
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            NavigationService.Source = new Uri("/ChatRoom.xaml", UriKind.Relative);
        }

        private void Friend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/Friend.xaml", UriKind.Relative);
        }

        private void Mypage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/MyPage.xaml", UriKind.Relative);
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.userId = null;
            NavigationService.Source = new Uri("/login.xaml", UriKind.Relative);
        }

        private void double_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is DataGridCell)
                {
                    var grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null
        && grid.SelectedItems.Count == 1)
                    {
                        var rowView = grid.SelectedItem as DataRowView;
                        if (rowView != null)
                        {
                            DataRow row = rowView.Row;
                            //do something with the underlying data
                        }
                    }
                }
            }
        }
    }
}
