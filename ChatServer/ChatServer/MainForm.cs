using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class MainForm : Form
    {
        TcpListener server = null;
        TcpClient client = null;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        public MainForm()
        {
            InitializeComponent();

            // socket start
            Thread tr = new Thread(InitSocket);
            tr.IsBackground = true;
            tr.Start();
        }

        private void InitSocket()
        {
            server = new TcpListener(IPAddress.Any, 9999);
            client = default(TcpClient);
            server.Start();
            Print(">> Server Started");

            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    Print(">> Accept connection from client");

                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$"));

                    clientList.Add(client, user_name);

                    // send message all user
                    SendMessageAll(user_name + " Joined ", "", false);

                    Controller ctr = new Controller();
                    ctr.OnReceived += new Controller.MessageDisplayHandler(OnReceived);
                    ctr.OnDisconnected += new Controller.DisconnectedHandler(h_client_OnDisconnected);
                    ctr.start(client, clientList);
                }
                catch (SocketException se)
                {
                    Trace.WriteLine(string.Format("InitSocket - SocketException : {0}", se.Message));
                    break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("InitSocket - Exception : {0}", ex.Message));
                    break;
                }
            }

            client.Close();
            server.Stop();
        }

        void h_client_OnDisconnected(TcpClient client)
        {
            if (clientList.ContainsKey(client))
            {
                clientList.Remove(client);
                Print(">> Disconnected connection from client");
            }

        }

        private void OnReceived(string message, string user_name)
        {
            string displayMessage = "From client : " + user_name + " : " + message;
            Print(displayMessage);
            SendMessageAll(message, user_name, true);
        }

        public void SendMessageAll(string message, string user_name, bool flag)
        {
            foreach (var pair in clientList)
            {
                Trace.WriteLine(string.Format("tcpclient : {0} user_name : {1}", pair.Key, pair.Value));

                TcpClient client = pair.Key as TcpClient;
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

        private void Print(string text)
        {
            if (console.InvokeRequired)
            {
                console.BeginInvoke(new MethodInvoker(delegate
                {
                    console.AppendText(text + Environment.NewLine);
                }));
            }
            else
                console.AppendText(text + Environment.NewLine);
        }
    }
}
