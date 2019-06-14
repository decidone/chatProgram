using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Login : Form
    {
        public static TcpClient client = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        //string message = string.Empty;
        Thread tr;
        Boolean get = true;

        public Login()
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

        private void btnLogin_Click(object sender, EventArgs e)
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            get = false;
            //tr.Join(); // 스레드 종료
            this.Visible = false; // 현재 폼 안보이게 하기
            Register frm = new Register(); // 새 폼 생성
            frm.Owner = this; // 새 폼의 오너를 현재 폼으로
            frm.Show(); // 새폼 보여 주 기 
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
                    
                    //string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //DisplayText(message);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format(ex.Message));
            }
        }

        //private void DisplayText(string text)
        //{
        //    if (richTextBox1.InvokeRequired)
        //    {
        //        richTextBox1.BeginInvoke(new MethodInvoker(delegate
        //        {
        //            richTextBox1.AppendText(text + Environment.NewLine);
        //        }));
        //    }
        //    else
        //        richTextBox1.AppendText(text + Environment.NewLine);
        //}
    }
}
