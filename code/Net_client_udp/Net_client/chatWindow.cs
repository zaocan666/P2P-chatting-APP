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
using System.Timers;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Net_client
{
    public partial class chatWindow : Form
    {
        public chatWindow()
        {
            InitializeComponent();
        }

        public byte[] buffer_Chat_receive = new byte[1024*1024*110];//110M缓存
        string his_ip;
        string my_ip;
        int his_port;
        int my_port;
        string his_name;
        string my_name;
        Thread receive_tr;
        UdpClient receive_sock;
        UdpClient send_sock;

        bool[] ack_received = new bool[10000];
        int send_ack_num=0;
        int receive_ack_num = 0;
        List<string> sent_messages = new List<string>();

        public chatWindow(string in_his_ip, string in_my_ip, string in_his_name, string in_my_name, Net_class sock)
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(chatWindow_FormClosing); //注册窗口关闭事件

            listView_message.Columns[0].Width = 100;
            listView_message.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView_message.Columns[1].Width = listView_message.Width- listView_message.Columns[0].Width;

            his_port = sock.get_his_port();
            my_port = sock.get_my_port();
            his_ip = in_his_ip;
            my_ip = in_my_ip;
            his_name = in_his_name;
            my_name = in_my_name;

            this.Text = "聊天：" + his_name;

            sock.Close();

            receive_sock = new UdpClient(my_port);
            send_sock = new UdpClient();
            
            CheckForIllegalCrossThreadCalls = false;
            receive_tr = new Thread(new ThreadStart(clientChat_receive));
            receive_tr.IsBackground = true;
            receive_tr.Start();
            
        }

        //发送消息
        private void button_send_Click(object sender, EventArgs e)
        {
            string text_send = textBox_message.Text;

            byte[] buf = Encoding.Default.GetBytes("1"+ my_name + "_" + text_send);
            IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(his_ip), his_port);
            send_sock.BeginSend(buf, buf.Length, serverAddr, null, null);
            sent_messages.Add(text_send);

            System.Timers.Timer t = new System.Timers.Timer(5000); //等待ACK，5秒的延时
            int now_ack_num = send_ack_num;
            t.Elapsed += new ElapsedEventHandler((s, ex) => ack_timeout(s, ex, now_ack_num));
            ack_received[send_ack_num] = false;
            send_ack_num++;
            t.AutoReset = false;
            t.Enabled = true;

            add_msg2list("我", text_send, Color.Green);
            textBox_message.Text = "";//清空消息输入框
        }

        //监听消息
        public void clientChat_receive()
        {
            IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] buf = receive_sock.Receive(ref from);
                string message = Encoding.Default.GetString(buf);
                
                //聊天信息
                if (message.Length > 1 && message[0] == '1')
                {
                    message = message.Substring(1);
                    string his_name = (message.Split('_')[0]);
                    add_msg2list(his_name, message.Split('_')[1], Color.Black);

                    byte[] send_buf = Encoding.Default.GetBytes("ack_" + receive_ack_num.ToString()); //发送ack
                    receive_ack_num++;
                    IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(his_ip), his_port);
                    send_sock.BeginSend(send_buf, send_buf.Length, serverAddr, null, null);
                }
                else if(message.Length > 1 && message.StartsWith("ack_")) //接到ack
                {
                    if(int.TryParse(message.Substring(4), out int get_num))
                    {
                        if(get_num>= send_ack_num || send_ack_num < 0)
                        {
                            return;
                        }

                        ack_received[get_num] = true;
                    }
                }
            }
        }

        public void ack_timeout(object sender, ElapsedEventArgs e, int ack_num)
        {
            if(ack_received[ack_num]==false)
            {
                string message = sent_messages[ack_num];
                DialogResult dr=MessageBox.Show("消息:"+message+"发送失败，是否重新发送？", "错误", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if(dr==DialogResult.Yes)
                {
                    textBox_message.Text = message;
                }
            }
        }

        //在对话框中添加消息
        public void add_msg2list(string name, string content, Color color)
        {
            string[] list_str = new string[2];
            list_str[0] = name + ":";
            list_str[1] = content;

            ListViewItem lv_item = new ListViewItem();
            lv_item.ForeColor = color;
            lv_item.Text = list_str[0];
            lv_item.SubItems.Add(list_str[1]);
            listView_message.Items.Add(lv_item);
            listView_message.Columns[1].Width = -1;
            if(listView_message.Columns[1].Width<80)
            {
                listView_message.Columns[1].Width = 80;
            }
        }
        
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        //窗口关闭前关闭连接
        private void chatWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            receive_tr.Abort(); 
            //释放内存
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void textBox_message_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
