using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            clientSocket.Connect("127.0.0.1", 9999);
            stream = clientSocket.GetStream();

            message = "Connected to Chat Server";
            DisplayText(message);

            byte[] buffer = Encoding.Unicode.GetBytes(this.TB_Name.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DataPacket packet = new DataPacket();

            packet.Name = this.TB_Msg.Text;
            packet.Subject = "Asd";
            packet.Grade = 31;
            packet.Memo = "eew";
            byte[] buffer = new byte[Marshal.SizeOf(packet)];

            unsafe
            {
                fixed (byte* fixed_buffer = buffer)
                {
                    Marshal.StructureToPtr(packet, (IntPtr)fixed_buffer, false);
                }
            }

            stream.Write(buffer, 0, Marshal.SizeOf(packet));
            
            //byte[] buffer = Encoding.Unicode.GetBytes(this.TB_Msg.Text + "$");
            //stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void GetMessage()
        {
            while (true)
            {
                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                DisplayText(message);
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
