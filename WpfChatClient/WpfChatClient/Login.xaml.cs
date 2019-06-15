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
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Page
    {
        NetworkStream stream = MainWindow.client.GetStream();
        //Thread tr;
        public Login()
        {
            InitializeComponent();
            //client.Connect("127.0.0.1", 9999);
            //stream = client.GetStream();

            ////string message = "Connected to Chat Server";
            ////DisplayText(message);

            ////byte[] buffer = Encoding.Unicode.GetBytes("asd" + "$");
            ////stream.Write(buffer, 0, buffer.Length);
            ////stream.Flush();

            //tr = new Thread(GetJSON);
            //tr.IsBackground = true;
            //tr.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "login",
                user_id = "asdww",
                user_pw = "pw"
                //Email = "james@example.com",
                //Active = true,
                //CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                //Roles = new List<string>
                //{
                //    "User",
                //    "Admin"
                //}
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            //string json = "{\"Email\": \"asd\"}";

            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            //client.Connect("127.0.0.1", 9999);
            //stream = client.GetStream();

            //string message = "Connected to Chat Server";
            //DisplayText(message);

            //byte[] buffer = Encoding.Unicode.GetBytes(this.TB_Name.Text + "$");
            //stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();

            //Thread tr = new Thread(GetMessage);
            //tr.IsBackground = true;
            //tr.Start();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //tr.Abort();
            DataPacket dp = new DataPacket
            {
                work = "page_move"
            };
            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            NavigationService.Navigate(
              new Uri("/Register.xaml", UriKind.Relative)
            );
        }

        //private void GetJSON()
        //{
        //    try
        //    {
        //        byte[] buffer = new byte[(int)client.ReceiveBufferSize];
        //        string jsonData = string.Empty;
        //        int bytes = 0;
        //        stream = client.GetStream();
        //        while (true)
        //        {
        //            bytes = stream.Read(buffer, 0, buffer.Length);
        //            MessageBox.Show("dmdkdkdk");
        //            jsonData = Encoding.Unicode.GetString(buffer, 0, bytes);
        //            jsonData = jsonData.Substring(0, jsonData.IndexOf("$"));

        //            JObject jobj = JObject.Parse(jsonData);

        //            //string message = Encoding.Unicode.GetString(buffer, 0, bytes);
        //            //DisplayText(message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(string.Format(ex.Message));
        //    }
        //}
    }
}
