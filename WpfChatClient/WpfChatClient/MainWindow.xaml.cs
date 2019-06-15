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
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TcpClient client = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        //string message = string.Empty;
        Thread tr;

        public MainWindow()
        {
            InitializeComponent();
            client.Connect("127.0.0.1", 9999);
            stream = client.GetStream();

            //string message = "Connected to Chat Server";
            //DisplayText(message);

            //byte[] buffer = Encoding.Unicode.GetBytes("asd" + "$");
            //stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();

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
                    MessageBox.Show("dmdkdkdk");
                    jsonData = Encoding.Unicode.GetString(buffer, 0, bytes);
                    jsonData = jsonData.Substring(0, jsonData.IndexOf("$"));

                    JObject jobj = JObject.Parse(jsonData);

                    if (jobj["work"].ToString() == "error" || jobj["work"].ToString() == "register_re")
                    {
                        MessageBox.Show(jobj["message"].ToString());
                        //this.frame.Source = new Uri("Login.xaml", UriKind.Relative);
                    }
                    //string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //DisplayText(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ex.Message));
            }
        }
    }
}
