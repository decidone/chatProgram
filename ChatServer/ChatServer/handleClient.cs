﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ChatServer
{
    class handleClient
    {
        TcpClient client = null;
        public List<TcpClient> clientList = null;
        
        public void startClient(TcpClient clientSocket)
        {
            this.client = clientSocket;
            //this.clientList = clientList;

            Thread tr = new Thread(run);
            tr.IsBackground = true;
            tr.Start();
        }
        
        public delegate void MessageDisplayHandler(string text);
        public event MessageDisplayHandler Print;

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
                    
                    string print = "work = " + jobj["work"].ToString();
                    Print(print);

                    // 나중에 case문으로 변경할 것
                    if (jobj["work"].ToString() == "login")
                        login(jobj, stream);
                    if (jobj["work"].ToString() == "register")
                        register(jobj, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ex.Message));

                if (client != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(client);

                    client.Close();
                    stream.Close();
                }
            }
        }

        private void login(JObject jobj, NetworkStream stream)
        {
            if(jobj["user_id"].ToString() == "asdww")
            {
                Print("로그인 성공");
            }
        }

        private void register(JObject jobj, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "INSERT INTO user (user_id, user_pw, user_name) " +
                                "VALUES ('" + jobj["user_id"] + "', '" + jobj["user_pw"] + "', '" + jobj["user_name"] + "')";

                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                
                Print(jobj["user_id"].ToString());
                dp.work = "register_re";
                dp.message = "가입 성공";
            }
            catch(MySqlException ex)
            {
                Print("이미 가입된 아이디 생성 요청");
                dp.work = "error";
                dp.message = "이미 가입되어 있는 아이디입니다.";
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }

            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            MainForm.conn.Close();
        }
    }
}
