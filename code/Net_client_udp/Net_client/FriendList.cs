using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Net_client
{
    public partial class FriendList : Form
    {
        string myName;
        Net_class server_NC;
        public byte[] buffer_F = new byte[1024];
        public byte[] buffer_Chat = new byte[1024];
        int chat_receive_port = Net_class.client_listenChat_port + 1; //下一个聊天可用端口
        List<int> chatting_ports = new List<int>();  //正在运行的聊天窗口所用的端口号集
        Dictionary<string, string> Friends_ip = new Dictionary<string, string>();

        public FriendList()
        {
            InitializeComponent();
            
        }
        
        public FriendList(string in_myName, Net_class NC)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            
            listView_friendList.Columns[0].Width = 100;
            listView_friendList.Columns[1].Width = 120;
            listView_friendList.Columns[2].Width = 100;

            myName = in_myName;
            server_NC = NC;

            listView_friendList.DoubleClick += new EventHandler(listView_friendList_DoubleClick); //注册鼠标双击事件
            FormClosing += new FormClosingEventHandler(friendList_FormClosing); //注册窗口关闭事件

            Listen_message();
            
        }

        //开启监听
        public void Listen_message()
        {
            try
            {
                //添加好友的监听
                Net_class listenF_NC = new Net_class();
                string myIP = Net_class.getMy_ip();
                listenF_NC.bind_ip_port(myIP, Net_class.client_listenF_port);
                listenF_NC.sock.Listen(200);
                listenF_NC.sock.BeginAccept(new AsyncCallback(clientF_accepted), listenF_NC.sock);

                //聊天的监听
                Net_class listenChat_NC = new Net_class();
                listenChat_NC.bind_ip_port(myIP, Net_class.client_listenChat_port);
                listenChat_NC.sock.Listen(200);
                listenChat_NC.sock.BeginAccept(new AsyncCallback(clientChat_accepted), listenChat_NC.sock);
            }
            catch(SocketException e)
            {
                MessageBox.Show("端口被占用。"+e.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //聊天监听的callback
        public void clientChat_accepted(IAsyncResult asy_result)
        {
            Socket listen_socket = (Socket)asy_result.AsyncState;
            Socket receive_socket = listen_socket.EndAccept(asy_result);

            Net_class receive_net = new Net_class(receive_socket);
            int my_receive_port = receive_net.get_my_port();

            if (my_receive_port == Net_class.client_listenChat_port) //在聊天的监听端口接收到请求，则给对方返回一个本地可用的端口后并监听该端口号
            {
                chat_receive_port += 1;
                while (Net_class.portInUse(chat_receive_port)) //判断端口chat_receive_port是否被占用，如果是则加1
                {
                    chat_receive_port += 1;
                }
                receive_net.Send_message_asy(chat_receive_port.ToString()); //向对方发送可用端口号

                string myIP = Net_class.getMy_ip();
                Net_class listenChat_custom_NC = new Net_class();
                listenChat_custom_NC.bind_ip_port(myIP, chat_receive_port);
                listenChat_custom_NC.sock.Listen(200);
                listenChat_custom_NC.sock.BeginAccept(new AsyncCallback(clientChat_accepted), listenChat_custom_NC.sock);
            }
            else if(chatting_ports.FindIndex(x => x==my_receive_port)==-1) //不是正在聊天的窗口所发来的连接
            {
                //开启接收消息
                receive_socket.BeginReceive(buffer_Chat, 0, buffer_Chat.Length, SocketFlags.None, new AsyncCallback(clientChat_receive), receive_socket);
                
            }

            //开始接受下一个聊天请求
            listen_socket.BeginAccept(new AsyncCallback(clientChat_accepted), listen_socket);
        }
        
        //收到聊天消息的callback
        public void clientChat_receive(IAsyncResult asy_result)
        {
            Socket receive_socket = (Socket)asy_result.AsyncState;
            Net_class receive_NC = new Net_class(receive_socket);
            int my_receive_port = receive_NC.get_my_port();

            IPEndPoint friend_endP = (IPEndPoint)receive_socket.RemoteEndPoint;
            string friend_ip = friend_endP.Address.ToString();

            try
            {
                int len;
                string message = receive_NC.Receive_string(asy_result, buffer_Chat, out len);

                //聊天邀请
                if (message.Length > 2 && message.StartsWith("_c"))
                {
                    string his_name = message.Substring(2);
                    
                    string his_ip= friend_ip;
                    //Friends_ip.TryGetValue(his_name, out his_ip);

                    if (!receive_NC.sock.Connected)
                    {
                        DialogResult dr = MessageBox.Show(this, "与用户：" + his_name + "的聊天创建失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    chatting_ports.Add(my_receive_port);
                    addChatWin(his_ip, his_name, receive_NC);
                }
                
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.ToString(), "异常",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //添加好友的callback
        public void clientF_accepted(IAsyncResult asy_result)
        {
            Socket listen_socket = (Socket)asy_result.AsyncState;
            Socket receive_socket = listen_socket.EndAccept(asy_result);

            //接收连接的具体信息
            receive_socket.BeginReceive(buffer_F, 0, buffer_F.Length, SocketFlags.None, new AsyncCallback(clientF_receive), receive_socket);
            //开始接受下一个好友请求(异步)
            listen_socket.BeginAccept(new AsyncCallback(clientF_accepted), listen_socket);
        }

        //收到添加好友消息的callback
        public void clientF_receive(IAsyncResult asy_result)
        {
            Socket receive_socket = (Socket)asy_result.AsyncState;
            Net_class receive_NC = new Net_class(receive_socket);

            IPEndPoint friend_endP = (IPEndPoint)receive_socket.RemoteEndPoint;
            string friend_ip = friend_endP.Address.ToString();

            try
            {
                int len;
                string message = receive_NC.Receive_string(asy_result, buffer_F, out len);
                
                if(message.Length<3)
                {
                    return;
                }

                string friend_name = message.Substring(2);

                if (message.StartsWith("_f")) //收到好友申请
                {
                    DialogResult dr = MessageBox.Show(this, "用户:"+ friend_name+" 请求添加好友，是否同意？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    string friend_reply;

                    if (dr==DialogResult.Yes) //接收好友申请
                    {
                        friend_reply = "_y";
                        add_friend2list(friend_name, friend_ip);
                    }
                    else //拒绝好友申请
                    {
                        friend_reply = "_n";
                    }

                    Net_class NC_friend_agree = new Net_class(friend_ip, Net_class.client_listenF_port);

                    NC_friend_agree.try_connect(3000);
                    if (!NC_friend_agree.sock.Connected)
                    {
                        MessageBox.Show(this, "无法连接到对方网络，请稍后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    NC_friend_agree.Send_message(friend_reply + myName);//发送回复
                    if (dr == DialogResult.Yes) //添加到好友列表
                    {
                        add_friend2list(friend_name, friend_ip);
                    }
                }
                else if(message.StartsWith("_y")) //好友申请被接受
                {
                    DialogResult dr = MessageBox.Show(this, "用户:" + friend_name + " 已接受您的好友申请。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    add_friend2list(friend_name, friend_ip);
                }
                else if(message.StartsWith("_n"))
                {
                    DialogResult dr = MessageBox.Show(this, "用户:" + friend_name + " 已拒绝您的好友申请。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //receive_NC.sock.BeginReceive(buffer_F, 0, buffer_F.Length, SocketFlags.None, new AsyncCallback(clientF_receive), receive_NC.sock);
            }
            catch(SocketException e)
            {
                MessageBox.Show(e.ToString(), "异常",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //向好友列表以及Friends_ip中添加在线好友
        public void add_friend2list(string friend_name, string friend_ip)
        {
            string[] list_str = new string[3];
            list_str[0] = friend_name;
            list_str[1] = friend_ip;
            list_str[2] = "在线";

            ListView.ListViewItemCollection all_items = listView_friendList.Items;
            ListViewItem find_item = listView_friendList.FindItemWithText(friend_name);
            if (find_item != null) //好友已添加
            {
                int index_find = listView_friendList.Items.IndexOf(find_item);
                listView_friendList.Items[index_find] = new ListViewItem(list_str);
            }
            else
            {
                listView_friendList.Items.Add(new ListViewItem(list_str));
                Friends_ip.Add(friend_name, friend_ip); //若好友不在线，则ip地址为n
            }
        }

        //发起聊天
        private void listView_friendList_DoubleClick(object sender, EventArgs e)
        {
            if (listView_friendList.SelectedItems.Count == 0) return;
            
            string his_name = listView_friendList.SelectedItems[0].Text;
            
            string his_ip= Friends_ip[his_name];

            Net_class NC_chat_apply = new Net_class(his_ip, Net_class.client_listenChat_port);

            /*Thread thr = new Thread(() => MessageBox.Show("连接中。。。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)); //显示提示窗口但不将程序挂起
            thr.IsBackground = true;
            thr.Start();*/

            NC_chat_apply.try_connect(3000);
            if(!NC_chat_apply.sock.Connected)
            {
                DialogResult dr = MessageBox.Show(this, "连接对方网络失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string new_port_str = NC_chat_apply.Receive_string(); //接收对方传来的端口信息。
            Net_class NC_chat_new = new Net_class(his_ip, int.Parse(new_port_str));
            NC_chat_new.try_connect(3000);
            if (!NC_chat_new.sock.Connected)
            {
                DialogResult dr = MessageBox.Show(this, "连接对方网络失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NC_chat_new.Send_message("_c" + myName);

            addChatWin(his_ip, his_name, NC_chat_new);
        }

        void addChatWin(string his_ip, string his_name, Net_class NC)
        {
            //chatWindow CW = new chatWindow(his_ip, Net_class.getMy_ip(), his_name, myName, NC_chat_apply);
            //CW.Show();
            chatWindow CW = new chatWindow(his_ip, Net_class.getMy_ip(), his_name, myName, NC);
            //ChatWinDic.Add(his_name, CW);
            Thread Thread_friendList = new Thread(() => Application.Run(CW));
            Thread_friendList.SetApartmentState(ApartmentState.STA); //要加这句，否则不能打开fileopendialog
            Thread_friendList.Start();
        }

        private void button_addFriend_Click(object sender, EventArgs e)
        {
            addFriend aF = new addFriend();
            aF.ShowDialog();
            string friend_name = aF.friend_name;

            if(friend_name=="")
            {
                return;
            }
            
            server_NC.Send_message("q" + friend_name);//查询好友状态
            string receive_string = server_NC.Receive_string();
            
            if (receive_string == "n" || receive_string.StartsWith("Please"))
            {
                DialogResult dr = MessageBox.Show(this, "对方不在线，添加好友失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Net_class NC_friend_apply = new Net_class(receive_string, Net_class.client_listenF_port);
                
                NC_friend_apply.try_connect(3000);
                if (!NC_friend_apply.sock.Connected)
                {
                    DialogResult dr = MessageBox.Show(this, "无法连接到对方网络，请稍后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                /*try
                { NC_friend_apply.try_connect(); }
                catch(SocketException)
                {
                    DialogResult dr = MessageBox.Show(this, "无法连接到对方网络，请稍后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/

                NC_friend_apply.Send_message("_f" + myName);//发送好友申请

                Thread thr = new Thread(()=> MessageBox.Show("已发送好友申请！\r\n对方在线。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)); //显示提示窗口但不将程序挂起
                thr.IsBackground = true;
                thr.Start();
                
                //NC_friend_apply.Close();
                
            }
        }

        private void friendList_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                server_NC.Send_message("logout" + myName);
            }
            catch
            { }

            server_NC.Close();
        }
    }
}
