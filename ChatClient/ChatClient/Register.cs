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
    public partial class Register : Form
    {
        //TcpClient client = MainForm.client;
        NetworkStream stream = Login.client.GetStream();
        Thread tr;

        public Register()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(closing);

            tr = new Thread(GetJSON);
            tr.IsBackground = true;
            tr.Start();
        }

        // 종료시 프로그램 전체 종료
        private void closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                work = "register",
                user_id = this.textBox1.Text,
                user_pw = this.textBox2.Text,
                user_name = this.textBox3.Text
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

        }

        private void GetJSON()
        {
            //MessageBox.Show("getJSON 시작");
            try
            {
                byte[] buffer = new byte[(int)Login.client.ReceiveBufferSize];
                string jsonData = string.Empty;
                int bytes = 0;
                stream = Login.client.GetStream();
                while (true)
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    jsonData = Encoding.Unicode.GetString(buffer, 0, bytes);
                    jsonData = jsonData.Substring(0, jsonData.IndexOf("$"));

                    JObject jobj = JObject.Parse(jsonData);
                    
                    if (jobj["work"].ToString() == "error" || jobj["work"].ToString() == "register_re")
                    {
                        MessageBox.Show(jobj["message"].ToString());
                    }
                    //string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //DisplayText(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
                Trace.WriteLine(string.Format(ex.Message));
            }
        }
    }
}
