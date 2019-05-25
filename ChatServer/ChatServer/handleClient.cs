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
    class handleClient
    {
        TcpClient clientSocket = null;
        public Dictionary<TcpClient, string> clientList = null;

        public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
        {
            this.clientSocket = clientSocket;
            this.clientList = clientList;

            Thread t_hanlder = new Thread(doChat);
            t_hanlder.IsBackground = true;
            t_hanlder.Start();
        }

        public delegate void MessageDisplayHandler(string message, string user_name);
        public event MessageDisplayHandler OnReceived;

        public delegate void DisconnectedHandler(TcpClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        private void doChat()
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
                    stream = clientSocket.GetStream();
                    //bytes = stream.Read(buffer, 0, buffer.Length);
                    //msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    //msg = msg.Substring(0, msg.IndexOf("$"));
                    while (stream.Read(buffer, 0, Marshal.SizeOf(packet)) != 0)
                    {
                        unsafe
                        {
                            fixed (byte* fixed_buffer = buffer)
                            {
                                Marshal.PtrToStructure((IntPtr)fixed_buffer, packet);
                                OnReceived(packet.Name, "test");
                            }
                        }
                    }
                    //OnReceived(msg, clientList[clientSocket].ToString());
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message));

                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("doChat - Exception : {0}", ex.Message));

                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
        }

    }
}
