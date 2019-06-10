using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class handleClient
    {
        TcpClient client = null;
        public Dictionary<TcpClient, string> clientList = null;

        public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
        {
            this.client = clientSocket;
            this.clientList = clientList;

            Thread t = new Thread(doChat);
            t.IsBackground = true;
            t.Start();
        }

        //public delegate void MessageDisplayHandler(string message, string user_name);
        public delegate void MessageDisplayHandler(string text);
        public event MessageDisplayHandler OnReceived;

        public delegate void DisconnectedHandler(TcpClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        private void doChat()
        {
            NetworkStream stream = null;
            try
            {
                byte[] buffer = new byte[(int)client.ReceiveBufferSize];
                string msg = string.Empty;
                int bytes = 0;
                int MessageCount = 0;

                while (true)
                {
                    MessageCount++;
                    stream = client.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    msg = msg.Substring(0, msg.IndexOf("$"));

                    JObject jobj = JObject.Parse(msg);
                    string print = jobj["Work"].ToString();
                    if (OnReceived != null)
                        //OnReceived(msg, clientList[clientSocket].ToString());
                        OnReceived(print);
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message));

                if (client != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(client);

                    client.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("doChat - Exception : {0}", ex.Message));

                if (client != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(client);

                    client.Close();
                    stream.Close();
                }
            }
        }

    }
}
