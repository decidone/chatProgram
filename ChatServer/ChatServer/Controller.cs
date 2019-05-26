using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class Controller
    {
        TcpClient client = null;
        public Dictionary<TcpClient, string> clientList = null;

        public void start(TcpClient client, Dictionary<TcpClient, string> clientList)
        {
            this.client = client;
            this.clientList = clientList;

            Thread tr = new Thread(runServe);
            tr.IsBackground = true;
            tr.Start();
        }

        public delegate void MessageDisplayHandler(string message, string user_name);
        public event MessageDisplayHandler OnReceived;

        public delegate void DisconnectedHandler(TcpClient client);
        public event DisconnectedHandler OnDisconnected;

        private void runServe()
        {
            //OnReceived("asd", "test"); 돌아감
            NetworkStream stream = null;
            try
            {
                //OnReceived("asd", "test"); 돌아감
                //byte[] buffer = new byte[1024];
                //string msg = string.Empty;
                //int bytes = 0;
                //int MessageCount = 0;
                byte[] buffer = new byte[8092];
                DataPacket packet = new DataPacket();
                while (true)
                {
                    //MessageCount++;
                    stream = client.GetStream();
                    //bytes = stream.Read(buffer, 0, buffer.Length);
                    //msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //msg = msg.Substring(0, msg.IndexOf("$"));
                    if (stream.Read(buffer, 0, Marshal.SizeOf(packet)) != 0)
                    {
                        unsafe
                        {
                            fixed (byte* fixed_buffer = buffer)
                            {
                                Marshal.PtrToStructure((IntPtr)fixed_buffer, packet);
                                OnReceived("test", packet.Name);
                                //break;
                            }
                        }
                    }
                    stream.Flush();
                    if (packet.Name.Equals("qwer"))
                    {
                        OnReceived("성공", packet.Name);
                    }
                    OnReceived("test2", packet.Name);
                    //OnReceived(packet.Name, "ww"); 안돌아감
                    //OnReceived(msg, clientList[clientSocket].ToString());
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
