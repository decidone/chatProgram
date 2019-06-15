using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ChatServer
{
    public partial class MainForm : Form
    {
        TcpListener server = null;
        TcpClient client = null;
        public static MySqlConnection conn = new MySqlConnection("Server=localhost; Database=chat_program; Uid=root; Pwd=cs1234;");
        //public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();
        public List<TcpClient> clientList = new List<TcpClient>();

        public MainForm()
        {
            InitializeComponent();

            // socket start
            Thread t = new Thread(InitSocket);
            t.IsBackground = true;
            t.Start();
        }

        private void InitSocket()
        {
            server = new TcpListener(IPAddress.Any, 9999);
            client = default(TcpClient);
            server.Start();
            DisplayText(">> Server Started");

            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();

                    //나중에 로그인 생기면 이것도 필요없을듯
                    DisplayText(">> Accept connection from client");
                    
                    //NetworkStream stream = client.GetStream();
                    //byte[] buffer = new byte[(int)client.ReceiveBufferSize];
                    //int bytes = stream.Read(buffer, 0, buffer.Length);
                    //string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //user_name = user_name.Substring(0, user_name.IndexOf("$"));

                    clientList.Add(client);

                    //// send message all user
                    //SendMessageAll(user_name + " Joined ", "", false);

                    handleClient h_client = new handleClient();
                    h_client.OnReceived += new handleClient.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new handleClient.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.startClient(client);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(string.Format("InitSocket - SocketException : {0}", se.Message));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("InitSocket - Exception : {0}", ex.Message));
                    break;
                }
            }

            client.Close();
            server.Stop();
        }

        void h_client_OnDisconnected(TcpClient clientSocket)
        {
            if (clientList.Contains(clientSocket))
            {
                clientList.Remove(clientSocket);
                DisplayText(">> Disconnected connection from client");
            }

        }
        
        private void OnReceived(string text)
        {
            //string displayMessage = "From client : " + user_name + " : " + message;
            //DisplayText(displayMessage);
            //SendMessageAll(message, user_name, true);
            DisplayText(text);
        }

        public void SendMessageAll(string message, string user_name, bool flag)
        {
            foreach (var pair in clientList)
            {
                Trace.WriteLine(string.Format("tcpclient : {0}", pair));

                TcpClient client = pair as TcpClient;
                NetworkStream stream = client.GetStream();
                byte[] buffer = null;

                if (flag)
                {
                    buffer = Encoding.Unicode.GetBytes(user_name + " says : " + message);
                }
                else
                {
                    buffer = Encoding.Unicode.GetBytes(message);
                }

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        private void DisplayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
                richTextBox1.AppendText(text + Environment.NewLine);
        }
    }
}
