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
using System.IO;

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
        string his_name;
        string my_name;
        string file_name;
        Net_class Sock_connected;
        
        public chatWindow(string in_his_ip, string in_my_ip, string in_his_name, string in_my_name, Net_class sock)
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(chatWindow_FormClosing); //注册窗口关闭事件

            listView_message.Columns[0].Width = 100;
            listView_message.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView_message.Columns[1].Width = listView_message.Width- listView_message.Columns[0].Width;

            his_ip = in_his_ip;
            my_ip = in_my_ip;
            his_name = in_his_name;
            my_name = in_my_name;
            Sock_connected = sock;

            this.Text = "聊天：" + his_name;

            sock.sock.BeginReceive(buffer_Chat_receive, 0, buffer_Chat_receive.Length, SocketFlags.None, new AsyncCallback(clientChat_receive), sock.sock); //异步接收
        }

        //发送消息
        private void button_send_Click(object sender, EventArgs e)
        {
            if(!Sock_connected.sock.Connected)
            {
                MessageBox.Show("对方已退出聊天窗口，聊天结束。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Sock_connected.Close();
                Close();
                return;
            }
            
            string text_send = textBox_message.Text;
            //Sock_connected.Send_message_asy("_m"+my_name+"_"+text_send);
            Sock_connected.Send_message_NumString_asy(1, my_name + "_" + text_send);

            add_msg2list("我", text_send, Color.Green);
            textBox_message.Text = "";//清空消息输入框
        }

        //收到消息
        public void clientChat_receive(IAsyncResult asy_result)
        {
            Net_class receive_NC;
            Socket receive_socket;

            if(!Sock_connected.sock.Connected)
            {
                Sock_connected.Close();
                Close();
                return;
            }

            try
            {
                receive_socket = (Socket)asy_result.AsyncState;
                receive_NC = new Net_class(receive_socket);

                IPEndPoint friend_endP = (IPEndPoint)receive_socket.RemoteEndPoint;
                string friend_ip = friend_endP.Address.ToString();
            }
            catch
            {
                return;
            }
            
            try
            {
                int receive_len;
                string message = receive_NC.Receive_string(asy_result, buffer_Chat_receive, out receive_len);

                //聊天信息
                if (message.Length > 1 && message[0] == 1)
                {
                    message = message.Substring(1);
                    string his_name = (message.Split('_')[0]);
                    add_msg2list(his_name, message.Split('_')[1], Color.Black);
                }
                //收到文件
                else if (receive_len > 1 && buffer_Chat_receive[0] == 2)
                {
                    string file_suffix = file_name.Substring(file_name.LastIndexOf('.'));

                    DialogResult re = MessageBox.Show("对方发来文件："+file_name+"，是否接收？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if(re == DialogResult.No)
                    {
                        return;
                    }

                    SaveFileDialog save_file_Dialog = new SaveFileDialog()
                    {
                        Filter = "(*" + file_suffix + ")|*" + file_suffix + "",
                        FileName = file_name
                    };

                    string savePath;
                    if (save_file_Dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        savePath = save_file_Dialog.FileName;

                        using (FileStream fs = new FileStream(savePath, FileMode.Append, FileAccess.Write))
                        {
                            fs.Write(buffer_Chat_receive, 1, receive_len - 1);
                            fs.Flush();
                            fs.Close();
                        }

                        add_msg2list("系统消息：", "接受到的文件已经保存：" + savePath, Color.Red);
                    }
                    
                }
                //收到文件名
                else if (receive_len > 1 && buffer_Chat_receive[0] == 3)
                {
                    message = message.Substring(1);
                    file_name = message;
                }

                receive_socket.BeginReceive(buffer_Chat_receive, 0, buffer_Chat_receive.Length, SocketFlags.None, new AsyncCallback(clientChat_receive), receive_socket);
            }
            catch (SocketException e)
            {
                //MessageBox.Show(e.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //发送文件
        private void button_file_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file_name = openFileDialog1.SafeFileName;//文件名
                string file_path = openFileDialog1.FileName;//文件全路径

                long file_length = new FileInfo(file_path).Length; //以字节为单位
                if(file_length>100*1000*1000) //超过100M
                {
                    MessageBox.Show("文件大小不能超过100M", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if(file_length<=0)
                {
                    MessageBox.Show("文件为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Sock_connected.Send_message_NumString(3, file_name);

                add_msg2list("系统消息", "正在发送文件:" + file_path, Color.Red);

                byte[] buffer_Chat_send = new byte[1024 * 1024 * 110];//110M缓存

                using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read))
                {
                    int readLength = 0;
                    //bool firstRead = true;
                    //long sentFileLength = 0;
                    
                    readLength = fs.Read(buffer_Chat_send, 0, buffer_Chat_send.Length);
                    Sock_connected.Send_message_NumByte(2, buffer_Chat_send, readLength);

                    /*while ((readLength = fs.Read(buffer_Chat_send, 0, buffer_Chat_send.Length)) > 0 && sentFileLength < file_length)
                    {
                        sentFileLength += readLength;

                        //第一次发送的字节流上加前缀
                        if (firstRead)
                        {
                            //第一个字节标记2，代表为文件
                            Sock_connected.Send_message_NumByte(2, buffer_Chat_send, readLength);
                            firstRead = false;
                        }
                        Sock_connected.sock.Send(buffer_Chat_send, 0, readLength, SocketFlags.None);
                    }*/

                    fs.Close();
                }

            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        //窗口关闭前关闭连接
        private void chatWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Sock_connected.Close();

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
