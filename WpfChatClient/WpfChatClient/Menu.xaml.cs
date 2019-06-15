using System;
using System.Collections.Generic;
using System.Linq;
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
        public Menu()
        {
            InitializeComponent();
        }

        private void Newchat_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/NewChat.xaml", UriKind.Relative);
        }

        private void Friend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/Friend.xaml", UriKind.Relative);
        }

        private void Mypage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/MyPage.xaml", UriKind.Relative);
        }
    }
}
