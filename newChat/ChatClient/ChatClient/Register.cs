using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Register : Form
    {
        //TcpClient client = MainForm.client;
        NetworkStream stream = MainForm.client.GetStream();
        public Register()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(closing);
        }

        private void closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataPacket dp = new DataPacket
            {
                Work = "login",
                Id = "asdww",
                Password = "pw"
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
    }
}
