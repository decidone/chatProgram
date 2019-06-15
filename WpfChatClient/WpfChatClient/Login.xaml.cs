using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    public partial class Login : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();
        
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "login",
                user_id = "asdww",
                user_pw = "pw"
                //Roles = new List<string>
                //{
                //    "User",
                //    "Admin"
                //}
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source = new Uri("/Register.xaml", UriKind.Relative);
        }
    }
}
