using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace WpfChatClient
{
    public partial class MainWindow : Window
    {
        public static TcpClient client = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        static public string userId;
        Thread tr;
        
        public MainWindow()
        {
            InitializeComponent();
            frame.Source = new Uri("Login.xaml", UriKind.Relative);
            client.Connect("127.0.0.1", 9999);
            stream = client.GetStream();
            

            tr = new Thread(GetJSON);
            tr.IsBackground = true;
            tr.Start();
        }

        private void GetJSON()
        {
            try
            {
                byte[] buffer = new byte[(int)client.ReceiveBufferSize];
                string jsonData = string.Empty;
                int bytes = 0;
                stream = client.GetStream();
                while (true)
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    jsonData = Encoding.Unicode.GetString(buffer, 0, bytes);
                    jsonData = jsonData.Substring(0, jsonData.IndexOf("$"));
                    JObject jobj = JObject.Parse(jsonData);
                    DataPacket des_json = JsonConvert.DeserializeObject<DataPacket>(jobj.ToString());
                    // 나중에 case문으로 변경할 것
                    if (des_json.work == "error")
                    {
                        MessageBox.Show(des_json.message);
                    }
                    if(des_json.work == "register_re")
                    {
                        MessageBox.Show(des_json.message);
                        Move("Login");
                    }
                    if (des_json.work == "login_re")
                    {
                        MessageBox.Show(des_json.message);
                        userId = des_json.user_id;
                        Move("Menu");
                    }
                    if (des_json.work == "add_friend_re")
                    {
                        MessageBox.Show(des_json.message);
                    }
                    //if (jobj["work"].ToString() == "friend_list_re")
                    //{
                    //    MessageBox.Show(jobj.ToString());
                    //    //Account account = JsonConvert.DeserializeObject<Account>(json);
                    //    ////FriendData.DataSource.Clear();
                    //    //if (jobj["friend_list"].Count == 0)
                    //    //{

                    //    //}
                    //    string friend_list = jobj["friend_list"][0].ToString();

                    //    FriendData.DataSource.Add(new FriendData(friend_list));
                    //    FriendData.DataSource.Add(new FriendData("asdasdasd"));
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ex.Message));
            }
        }

        private void Move(string path)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                frame.Source = new Uri(path + ".xaml", UriKind.Relative);
            }));
        }
    }
}
