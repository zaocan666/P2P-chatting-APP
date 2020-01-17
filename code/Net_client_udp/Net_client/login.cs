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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            Net_class NC = new Net_class(textBox_serverIP.Text, int.Parse(textBox_serverPort.Text)); //自定义类，处理网络事件
            
            NC.try_connect(6000);
            if (!NC.sock.Connected)
            {
                DialogResult dr = MessageBox.Show(this, "无法连接到服务器，请检查服务器信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NC.Close();
                return;
            }

            string user_name = textBox_userName.Text;
            NC.Send_message(user_name + "_" + textBox_password.Text);//发送用户名密码
            
            string receive_string = NC.Receive_string();

            if (receive_string == "lol")
            {
                DialogResult dr = MessageBox.Show(this,"登录成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dr = MessageBox.Show(this,"登录失败，请检查用户名密码。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread Thread_friendList = new Thread(() => Application.Run(new FriendList(user_name, NC)));
            Thread_friendList.Start();
            this.Close();
        }
    }
}
