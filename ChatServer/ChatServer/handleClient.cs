using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ChatServer
{
    class handleClient
    {
        TcpClient client = null;
        public void startClient(TcpClient clientSocket)
        {
            this.client = clientSocket;

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
                    DataPacket des_json = JsonConvert.DeserializeObject<DataPacket>(jobj.ToString());

                    string print = "work = " + jobj["work"].ToString();
                    Print(print);

                    // 나중에 case문으로 변경할 것
                    if (des_json.work == "login")
                        login(des_json, stream);
                    if (des_json.work == "register")
                        register(des_json, stream);
                    if (des_json.work == "add_friend")
                        add_friend(des_json, stream);
                    if (des_json.work == "friend_list")
                        friend_list(des_json, stream);
                    if (des_json.work == "del_friend")
                        del_friend(des_json, stream);
                    if (des_json.work == "new_chat")
                        new_chat(des_json, stream);
                    if (des_json.work == "invite_friend")
                        invite_friend(des_json, stream);
                    if (des_json.work == "chat_out")
                        chat_out(des_json, stream);
                    if (des_json.work == "chat_room_out")
                        chat_room_out(des_json, stream);
                    if (des_json.work == "chat_in")
                        chat_in(des_json, stream);
                    if (des_json.work == "send_message")
                        send_message(des_json, stream);
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
        #region login
        private void login(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            dp.work = "error";
            dp.message = "아이디나 패스워드를 확인하세요.";
            try
            {
                DataSet ds = new DataSet();
                string sql = "SELECT user_pw FROM user WHERE user.user_id = '"+ des_json.user_id + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "user");
                if (ds.Tables.Count > 0)
                {
                    dp.chat_list = new List<int>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        if(r["user_pw"].ToString() == des_json.user_pw)
                        {
                            dp.work = "login_re";
                            dp.message = "로그인 성공";
                            dp.user_id = des_json.user_id;
                            Print(des_json.user_id + " 로그인");
                            MainForm.clientList[client] = des_json.user_id;
                            //Print(des_json.user_id + " 로그인");
                        }
                    }
                }
                ds = new DataSet();
                sql = "SELECT room_num FROM chat_user WHERE user_id = '" + des_json.user_id + "'";
                adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "chat_user");
                if (ds.Tables.Count > 0)
                {
                    //dp.work = "friend_list_re";
                    dp.chat_list = new List<int>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        dp.chat_list.Add(Convert.ToInt32(r["room_num"]));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Print("MySql 에러 : " + ex.ToString());
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }

            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
        #endregion

        #region register
        private void register(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "INSERT INTO user (user_id, user_pw, user_name) " +
                                "VALUES ('" + des_json.user_id + "', '" + des_json.user_pw + "', '" + des_json.user_name + "')";

                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                
                Print(des_json.user_id.ToString());
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
        #endregion

        #region add_friend
        private void add_friend(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                if (des_json.user_id != des_json.friend_id)
                {
                    String sql = "INSERT INTO friend (user_id, friend_id) " +
                                                    "VALUES ('" + des_json.user_id + "', '" + des_json.friend_id + "')";
                    MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                    cmd.ExecuteNonQuery();
                    Print("친구 추가 : " + des_json.user_id + " : " + des_json.friend_id);
                    dp.work = "add_friend_re";
                    dp.message = "등록 완료";
                }
                else
                {
                    dp.work = "error";
                    dp.message = "친구추가 할 아이디를 다시 한 번 확인해주세요.";
                }
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "친구추가 할 아이디를 다시 한 번 확인해주세요.";
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
        #endregion

        #region friend_list
        private void friend_list(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            //dp.work = "error";
            //dp.message = "아이디나 패스워드를 확인하세요.";
            try
            {
                DataSet ds = new DataSet();
                string sql = "SELECT friend_id FROM friend WHERE user_id = '" + des_json.user_id + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "friend");
                if (ds.Tables.Count > 0)
                {
                    dp.work = "friend_list_re";
                    dp.friend_list = new List<string>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        dp.friend_list.Add(r["friend_id"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                Print("MySql 에러 : " + ex.ToString());
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }

            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
        #endregion

        #region del_friend
        private void del_friend(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "DELETE FROM friend WHERE user_id = '" + des_json.user_id+ "' AND friend_id = '" + des_json.friend_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                dp.work = "del_friend_re";
                dp.message = "삭제 완료";
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "삭제 실패";
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
        #endregion

        #region new_chat
        private void new_chat(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "INSERT INTO chat_room VALUE()";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();

                sql = "SELECT MAX(room_num) AS room_num from chat_room";
                cmd = new MySqlCommand(sql, MainForm.conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                string str_Temp = "";

                while (rdr.Read())
                {
                    str_Temp += rdr["room_num"].ToString();
                }
                rdr.Close();

                int roomNum = Convert.ToInt32(str_Temp);
                sql = "INSERT INTO chat_user VALUES('" + roomNum + "','" + des_json.user_id + "', '1')";
                cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                
                dp.work = "new_chat_re";
                dp.room_num = roomNum;
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "새로운 채팅방을 생성하지 못했습니다.";
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
        #endregion

        #region invite_friend
        private void invite_friend(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "INSERT INTO chat_user VALUES('" + des_json.room_num + "', '" + des_json.friend_id + "', '0')";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                //Print(des_json.room_num.ToString());
                dp.work = "invite_friend_re";
                dp.message = des_json.friend_id + "님을 초대했습니다.";
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "이미 채팅방에 초대되어있는 유저입니다.";
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
        #endregion

        #region chat_out
        private void chat_out(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "UPDATE chat_user SET user_in_room = '0' WHERE room_num = '" + des_json.room_num + "' AND user_id = '" + des_json.user_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                //Print(des_json.room_num.ToString());
                dp.work = "chat_out_re";
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "이미 채팅방에 초대되어있는 유저입니다.";
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
        #endregion

        #region chat_room_out
        private void chat_room_out(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "DELETE FROM chat_user WHERE room_num = '" + des_json.room_num + "' AND user_id = '" + des_json.user_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();
                //Print(des_json.room_num.ToString());
                dp.work = "chat_room_out_re";
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "이미 채팅방에서 나갔습니다.";
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
        #endregion

        #region chat_in
        private void chat_in(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            try
            {
                String sql = "UPDATE chat_user SET user_in_room = '1' WHERE room_num = '" + des_json.room_num + "' AND user_id = '" + des_json.user_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();
                sql = "SELECT user_id, chat_message, chat_time FROM chat WHERE room_num = '" + des_json.room_num + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "chat");
                if (ds.Tables.Count > 0)
                {
                    dp.chat = new List<Chat>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        dp.chat.Add(new Chat(r["user_id"].ToString(), r["chat_message"].ToString(), Convert.ToDateTime(r["chat_time"])));
                    }
                }
                
                dp.work = "chat_in_re";
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "채팅방에 입장할 수 없습니다.";
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
        #endregion

        #region send_message
        private void send_message(DataPacket des_json, NetworkStream stream)
        {
            DataPacket dp = new DataPacket();
            MainForm.conn.Open();
            List<string> user_in_room = new List<string>();
            try
            {
                String sql = "INSERT INTO chat(room_num, user_id, chat_message, chat_time) VALUES('" + des_json.room_num + "', '" + des_json.user_id + "', '" + des_json.message + "', now())";
                MySqlCommand cmd = new MySqlCommand(sql, MainForm.conn);
                cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();
                sql = "SELECT user_id, chat_message, chat_time FROM chat WHERE room_num = '" + des_json.room_num + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "chat");
                if (ds.Tables.Count > 0)
                {
                    dp.chat = new List<Chat>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        dp.chat.Add(new Chat(r["user_id"].ToString(), r["chat_message"].ToString(), Convert.ToDateTime(r["chat_time"])));
                    }
                }
                dp.work = "send_message_re";

                ds = new DataSet();
                sql = "SELECT user_id FROM chat_user WHERE room_num = '" + des_json.room_num + "' AND user_in_room = '1'";
                adpt = new MySqlDataAdapter(sql, MainForm.conn);
                adpt.Fill(ds, "chat_user");
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        user_in_room.Add(r["user_id"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                //Print(ex.ToString());
                dp.work = "error";
                dp.message = "채팅 수신오류";
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }
            MainForm.conn.Close();

            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);
            byte[] buffer = Encoding.Unicode.GetBytes(json + "$");
            foreach (var pair in MainForm.clientList)
            {
                for(int i = 0; i<user_in_room.Count; i++)
                {
                    if (pair.Value == user_in_room[i]) {
                        TcpClient client = pair.Key as TcpClient;
                        NetworkStream Stream = client.GetStream();
                        Stream.Write(buffer, 0, buffer.Length);
                        Stream.Flush();
                    }
                }
            }
        }
        #endregion
    }
}
