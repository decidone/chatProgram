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

            Thread tr = new Thread(run);
            tr.IsBackground = true;
            tr.Start();
        }

        //public delegate void MessageDisplayHandler(string message, string user_name);
        public delegate void MessageDisplayHandler(string text);
        public event MessageDisplayHandler OnReceived;

        public delegate void DisconnectedHandler(TcpClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        private void run()
        {
            NetworkStream stream = null;
            try
            {
                byte[] buffer = new byte[(int)client.ReceiveBufferSize];
                string jsonData = string.Empty;
                int bytes = 0;

                while (true)
                {
                    stream = client.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    jsonData = Encoding.Unicode.GetString(buffer, 0, bytes);
                    jsonData = jsonData.Substring(0, jsonData.IndexOf("$"));

                    JObject jobj = JObject.Parse(jsonData);
                    
                    string print = "Work = " + jobj["Work"].ToString();
                    OnReceived(print);

                    if (jobj["Work"].ToString() == "login")
                        login(jobj);
                }
            }
            //catch (SocketException se)
            //{
            //    Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message));

            //    if (client != null)
            //    {
            //        if (OnDisconnected != null)
            //            OnDisconnected(client);

            //        client.Close();
            //        stream.Close();
            //    }
            //}
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format(ex.Message));

                if (client != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(client);

                    client.Close();
                    stream.Close();
                }
            }
        }
        private void login(JObject jobj)
        {
            if(jobj["Id"].ToString() == "asdww")
            {
                OnReceived("로그인 성공");
            }
        }
    }
}
