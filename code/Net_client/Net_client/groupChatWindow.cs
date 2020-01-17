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
using System.Threading;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Net_client
{
    public partial class groupChatWindow : Form
    {
        static int name_len = 10; //通信协议规定名字长为10

        bool client_activeClose = true;//群组成员主动关闭窗口
        bool host_flag; //是否为房主
        Net_class host_connectNC; //与房主的连接
        Dictionary<string, Net_class> connected_name_NC; //身为房主，与其他用户的连接
        string my_name;
        public byte[] buffer_member_receive = new byte[1024 * 1024 * 110];//110M缓存
        List<Thread> running_thread = new List<Thread>();
        myFileInfo applying_file=new myFileInfo();

        Dictionary<myFileInfo, byte[]> host_file_list = new Dictionary<myFileInfo, byte[]>();
        List<myFileInfo> fileInfo_list = new List<myFileInfo>();

        public class myFileInfo
        {
            public long length;
            public string name;

            public myFileInfo()
            {
                length = 0;
                name = "";
            }

            public myFileInfo(long in_length, string in_name)
            {
                length = in_length;
                name = in_name;
            }

            public override bool Equals(object obj)
            {
                myFileInfo file_info = obj as myFileInfo;
                if(file_info.name==name && file_info.length==length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return (int)length+name.GetHashCode();
            }

            public static bool operator == (myFileInfo a, myFileInfo b) //运算符重载，判断两对象相等
            {
                if(a.length==b.length && a.name==b.name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool operator !=(myFileInfo a, myFileInfo b)
            {
                if (a.length != b.length || a.name != b.name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        

        //群主和成员共同的初始化代码
        public void common_init()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(groupChatWindow_FormClosing); //注册窗口关闭事件
            listView_message.Columns[0].Width = 100;
            listView_message.Columns[1].Width = 300;
            listView_file.DoubleClick += new EventHandler(listView_file_DoubleClick); //注册鼠标双击事件
        }

        //房主初始化
        public groupChatWindow(Dictionary<string, Net_class> connected, string my_name_in)
        {
            common_init();

            my_name = my_name_in;
            connected_name_NC = connected;
            host_flag = true;

            send_names2all();

            addMember2list("群主", my_name, Color.Green);
            foreach(string name in connected_name_NC.Keys)
            {
                addMember2list("成员", name, Color.Black);
                
                Thread threadWatch = new Thread(()=> host_receive(name, connected_name_NC[name]));
                threadWatch.IsBackground = true;
                threadWatch.Start();
                running_thread.Add(threadWatch);
            }
        }

        //成员初始化
        public groupChatWindow(Net_class connected, string my_name_in)
        {
            common_init();

            my_name = my_name_in;
            host_connectNC = connected;
            host_flag = false;

            host_connectNC.sock.BeginReceive(buffer_member_receive, 0, buffer_member_receive.Length, SocketFlags.None, new AsyncCallback(member_receive), host_connectNC.sock); //异步接收
        }

        //房主等待每位群成员的消息
        public void host_receive(string his_name, Net_class connected_NC)
        {
            myFileInfo last_fileInfo=new myFileInfo(0,"");

            while(true)
            {
                if(!connected_NC.sock.Connected)
                {
                    return;
                }

                int info_length;
                byte[] receive_info = connected_NC.Receive_bytes(out info_length);
                string msg = Encoding.UTF8.GetString(receive_info, 1, info_length - 1);

                //文字消息
                if(info_length>0&&receive_info[0]==0)
                {
                    add_msg2list(his_name, msg, Color.Black);
                    send_msg2all(his_name, msg, false);
                }
                //文件
                else if(info_length > 0 && receive_info[0] == 2)
                {
                    if(last_fileInfo.length==0)
                    {
                        continue;
                    }

                    byte[] file_bytes = new byte[info_length-1];
                    Buffer.BlockCopy(receive_info, 1, file_bytes, 0, info_length-1);
                    host_file_list.Add(last_fileInfo, file_bytes);
                    add_file2list(last_fileInfo, his_name, Color.Black);
                    send_fileInfo2all_users(last_fileInfo, his_name);
                }
                //文件长度和文件名
                else if(info_length>1&&receive_info[0]==3)
                {
                    string[] msg_split = msg.Split('_');
                    long file_len;
                    if(!long.TryParse(msg_split[0], out file_len))
                    {
                        continue;
                    }

                    string file_name = msg_split[1];
                    for(int i=2;i< msg_split.Length;i++)
                    {
                        file_name += ("_" + msg_split[i]);
                    }

                    last_fileInfo = new myFileInfo(file_len, file_name);
                }
                //用户退出消息
                else if (info_length > 1 && receive_info[0] == 4)
                {
                    if (msg == "q")
                    {
                        for(int i=1;i<listView_members.Items.Count;i++)
                        {
                            string item_name = listView_members.Items[i].SubItems[1].Text;
                            if(item_name == his_name)
                            {
                                listView_members.Items.Remove(listView_members.Items[i]);
                            }
                        }
                        connected_name_NC[his_name].Close();
                        connected_name_NC.Remove(his_name);
                        return;
                    }
                }
                //请求文件
                else if(info_length > 1 && receive_info[0] == 5)
                {
                    string[] msg_split = msg.Split('_');
                    myFileInfo get_fileInfo = new myFileInfo();
                    if (!long.TryParse(msg_split[0], out get_fileInfo.length))
                    {
                        continue;
                    }

                    get_fileInfo.name = msg_split[1];
                    for (int i = 2; i < msg_split.Length; i++)
                    {
                        get_fileInfo.name += ("_" + msg_split[i]);
                    }

                    byte[] file_byte;
                    if(!host_file_list.TryGetValue(get_fileInfo, out file_byte))
                    {
                        connected_name_NC[his_name].Send_message_NumString_asy(5, "n");
                    }
                    else
                    {
                        connected_name_NC[his_name].Send_message_NumByte_asy(2, file_byte, file_byte.Length);
                    }

                }
            }
        }

        //把群组成员的名字发给每个成员
        public void send_names2all()
        {
            string names = my_name;
            foreach (string name in connected_name_NC.Keys)
            {
                names += name;
            }

            send_num_msg2all(1, names);
        }

        //给所有成员发送消息msg
        public void send_msg2all(string his_name, string msg, bool host_msg)
        {
            foreach(string name in connected_name_NC.Keys)
            {
                if(name==his_name && (!host_msg))
                {
                    continue;
                }

                connected_name_NC[name].Send_message_NumString_asy(0, his_name + msg);
            }
        }

        //向所有用户发送num+msg，num是标志位，msg是消息
        public void send_num_msg2all(int num, string msg)
        {
            foreach (string name in connected_name_NC.Keys)
            {
                connected_name_NC[name].Send_message_NumString_asy(num, msg);
            }
        }

        public bool parse_file_msg(byte[] info, int length, out string his_name, out myFileInfo file_info)
        {
            string message = Encoding.UTF8.GetString(info, 1, length - 1);
            file_info = new myFileInfo();
            string[] msg_split = message.Split('_');
            bool success = long.TryParse(msg_split[0], out file_info.length);
            
            his_name = msg_split[1];
            file_info.name = msg_split[2];
            for(int i=3;i< msg_split.Length;i++)
            {
                file_info.name += ("_"+msg_split[i]);
            }
            return success;
        }

        public void parse_string_msg(byte[] info, int length, out string name, out string msg)
        {
            name = Encoding.UTF8.GetString(info, 1, name_len);
            msg = Encoding.UTF8.GetString(info, name_len+1, length - 1 -name_len);
        }

        //成员接收到信息的callback
        public void member_receive(IAsyncResult asy_result)
        {
            Net_class receive_NC;
            Socket receive_socket;

            if (!host_connectNC.sock.Connected)
            {
                host_connectNC.Close();
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
                string message = receive_NC.Receive_string(asy_result, buffer_member_receive, out receive_len);

                //聊天信息
                if (receive_len > 1 && message[0] == 0)
                {
                    parse_string_msg(buffer_member_receive, receive_len, out string his_name, out string msg);
                    add_msg2list(his_name, msg, Color.Black);
                }
                //收到成员名
                else if (receive_len > 1 && buffer_member_receive[0] == 1)
                {
                    listView_members.Items.Clear();//首先清空列表

                    int name_num = (receive_len - 1) / name_len;
                    string host_name = message.Substring(1,name_len);
                    addMember2list("群主", host_name, Color.Black);
                    for(int i=1;i<name_num;i++)
                    {
                        string his_name = message.Substring(1+name_len*i,name_len);
                        Color c = his_name == my_name ? Color.Green : Color.Black;
                        addMember2list("成员", his_name, c);
                    }
                }
                //收到文件
                else if (receive_len > 1 && buffer_member_receive[0] == 2)
                {
                    string file_suffix = applying_file.name.Substring(applying_file.name.LastIndexOf('.'));

                    DialogResult re = MessageBox.Show("文件已收到：" + applying_file.name + "，是否接收？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (re == DialogResult.No)
                    {
                        return;
                    }

                    SaveFileDialog save_file_Dialog = new SaveFileDialog()
                    {
                        Filter = "(*" + file_suffix + ")|*" + file_suffix + "",
                        FileName = applying_file.name
                    };

                    string savePath;
                    if (save_file_Dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        savePath = save_file_Dialog.FileName;

                        using (FileStream fs = new FileStream(savePath, FileMode.Append, FileAccess.Write))
                        {
                            fs.Write(buffer_member_receive, 1, receive_len - 1);
                            fs.Flush();
                            fs.Close();
                        }

                        add_msg2list("系统消息：", "接受到的文件已经保存：" + savePath, Color.Red);
                    }

                }
                //收到文件信息
                else if (receive_len > 1 && buffer_member_receive[0] == 3)
                {
                    if(parse_file_msg(buffer_member_receive, receive_len, out string his_name, out myFileInfo file_info))
                    {
                        add_file2list(file_info, his_name, Color.Black);
                    }
                }
                //群组解散
                else if(receive_len>1&&buffer_member_receive[0]==4)
                {
                    string msg = Encoding.UTF8.GetString(buffer_member_receive, 1, receive_len - 1);
                    if(msg=="q")
                    {
                        MessageBox.Show("群组已解散！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        client_activeClose = false;
                        host_connectNC.Close();
                        this.Close();
                        return;
                    }
                }
                //没有找到文件
                if (receive_len > 1 && message[0] == 5)
                {
                    add_msg2list("系统消息：", "没有找到文件:" + applying_file.name, Color.Red);
                }

                receive_socket.BeginReceive(buffer_member_receive, 0, buffer_member_receive.Length, SocketFlags.None, new AsyncCallback(member_receive), receive_socket);
            }
            catch (SocketException e)
            {
                //MessageBox.Show(e.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //在成员信息中添加信息
        public void addMember2list(string identity, string name, Color c)
        {
            string[] list_str = new string[2];
            list_str[0] = identity;
            list_str[1] = name;

            ListViewItem lv_item = new ListViewItem
            {
                ForeColor = c,
                Text = list_str[0]
            };
            lv_item.SubItems.Add(list_str[1]);
            listView_members.Items.Add(lv_item);
        }

        //在对话框中添加消息
        public void add_msg2list(string name, string content, Color color)
        {
            string[] list_str = new string[2];
            list_str[0] = name + ":";
            list_str[1] = content;

            ListViewItem lv_item = new ListViewItem
            {
                ForeColor = color,
                Text = list_str[0]
            };
            lv_item.SubItems.Add(list_str[1]);
            listView_message.Items.Add(lv_item);

            listView_message.Columns[1].Width = -1;
            if (listView_message.Columns[1].Width < 80)
            {
                listView_message.Columns[1].Width = 80;
            }
        }

        //在文件栏中添加消息
        public void add_file2list(myFileInfo file_info, string his_name, Color color)
        {
            fileInfo_list.Add(file_info);

            string file_len_str;
            if(file_info.length<1000) //B量级
            {
                file_len_str = file_info.length.ToString() + "B";
            }
            else if(file_info.length < 1000*1000) //KB量级
            {
                file_len_str = (file_info.length/1000).ToString() + "KB";
            }
            else //MB量级
            {
                file_len_str = (file_info.length / (1000*1000)).ToString() + "MB";
            }

            string[] list_str = new string[3];
            list_str[0] = his_name;
            list_str[1] = file_len_str;
            list_str[2] = file_info.name;

            ListViewItem lv_item = new ListViewItem
            {
                ForeColor = color,
                Text = list_str[0]
            };
            lv_item.SubItems.Add(list_str[1]);
            lv_item.SubItems.Add(list_str[2]);
            listView_file.Items.Add(lv_item);

            listView_file.Columns[2].Width = -1;
        }

        //请求文件
        private void listView_file_DoubleClick(object sender, EventArgs e)
        {
            if (listView_file.SelectedItems.Count == 0) return;

            ListViewItem lv_item = listView_file.SelectedItems[0];
            int index = lv_item.Index;
            applying_file = fileInfo_list[index];

            if (host_flag)
            {
                byte[] file_info;
                if(!host_file_list.TryGetValue(applying_file, out file_info))
                {
                    MessageBox.Show("没有找到文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string savePath;
                string file_suffix = applying_file.name.Substring(applying_file.name.LastIndexOf('.'));
                SaveFileDialog save_file_Dialog = new SaveFileDialog()
                {
                    Filter = "(*" + file_suffix + ")|*" + file_suffix + "",
                    FileName = applying_file.name
                };
                if (save_file_Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    savePath = save_file_Dialog.FileName;

                    using (FileStream fs = new FileStream(savePath, FileMode.Append, FileAccess.Write))
                    {
                        fs.Write(file_info, 0, file_info.Length);
                        fs.Flush();
                        fs.Close();
                    }

                    add_msg2list("系统消息：", "请求的文件已经保存：" + savePath, Color.Red);
                }
            }
            else
            {
                host_connectNC.Send_message_NumString(5, applying_file.length.ToString() + '_' + applying_file.name);
                add_msg2list("系统消息", "正在下载文件:" + applying_file.name, Color.Red);
            }
            
        }
        
        //发送键按下
        private void button_send_Click(object sender, EventArgs e)
        {
            string text_send = textBox_message.Text;
            add_msg2list("我", text_send, Color.Green);

            if(host_flag) //是房主
            {
                send_msg2all(my_name, text_send, true);
            }
            else
            {
                if(!host_connectNC.sock.Connected)
                {
                    MessageBox.Show("群组已解散！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

                //向房主发消息不用发名字
                host_connectNC.Send_message_NumString_asy(0, text_send);
            }

            textBox_message.Text = "";
        }

        public void send_fileInfo2all_users(myFileInfo file_info, string his_name)
        {
            foreach (string name in connected_name_NC.Keys)
            {
                connected_name_NC[name].Send_message_NumString_asy(3, file_info.length.ToString()+ '_' + his_name + '_' + file_info.name);
            }
        }

        private void button_sendFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openf_d = new OpenFileDialog();
            if (openf_d.ShowDialog() == DialogResult.OK)
            {
                string file_name = openf_d.SafeFileName;//文件名
                string file_path = openf_d.FileName;//文件全路径

                long file_length = new FileInfo(file_path).Length; //以字节为单位
                if (file_length > 30 * 1000 * 1000) //超过100M
                {
                    MessageBox.Show("文件大小不能超过30M", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (file_length <= 0)
                {
                    MessageBox.Show("文件为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                myFileInfo file_info = new myFileInfo(file_length, file_name);

                if (!host_flag)
                {
                    if(fileInfo_list.Exists(x=>x==file_info))
                    {
                        MessageBox.Show("文件已存在。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    host_connectNC.Send_message_NumString(3, file_length.ToString()+'_'+file_name);

                    add_msg2list("系统消息", "正在上传文件:" + file_path, Color.Red);

                    byte[] buffer_file_send = new byte[1024 * 1024 * 35];//35M缓存

                    using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read))
                    {

                        int readLength = fs.Read(buffer_file_send, 0, buffer_file_send.Length);
                        host_connectNC.Send_message_NumByte(2, buffer_file_send, readLength);
                        
                        fs.Close();
                    }
                }
                else
                {
                    if(host_file_list.Keys.Contains(file_info))
                    {
                        MessageBox.Show("文件已存在。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    byte[] buffer_file_send = new byte[1024 * 1024 * 35];//35M缓存

                    add_msg2list("系统消息", "正在上传文件:" + file_path, Color.Red);

                    using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read))
                    {
                        int readLength = 0;

                        readLength = fs.Read(buffer_file_send, 0, buffer_file_send.Length);
                        byte[] file_bytes = new byte[readLength];
                        Buffer.BlockCopy(buffer_file_send, 0, file_bytes, 0, readLength);
                        host_file_list.Add(file_info, file_bytes);

                        fs.Close();
                    }
                    
                    send_fileInfo2all_users(file_info, my_name);
                    add_file2list(file_info, my_name, Color.Black);
                }

            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        //窗口关闭前关闭连接
        private void groupChatWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (host_flag) //是房主
            {
                send_num_msg2all(4, "q");
            }
            else if (client_activeClose) //成员主动关闭窗口
            {
                host_connectNC.Send_message_NumString(4, "q");
            }

            for (int i = 0; i < running_thread.Count; i++)
            {
                running_thread[i].Abort();
            }

            //释放内存
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
    }
}
