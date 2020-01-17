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

namespace Net_server
{
    public partial class Form1 : Form
    {
        Socket socket_watch;
        Dictionary<string, string> ip_online = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
            txtMsg.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;

            string myIP = get_my_ip();
            textBox_localIP.Text = myIP;

            IPAddress address = IPAddress.Parse(myIP);
            IPEndPoint endPoint = new IPEndPoint(address, 8000);

            socket_watch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket_watch.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket_watch.Bind(endPoint);
            }
            catch (SocketException se)
            {
                MessageBox.Show("异常：" + se.Message);
                return;
            }

            socket_watch.Listen(1000);
            Thread threadWatch = new Thread(Accept_connect);
            threadWatch.IsBackground = true;
            threadWatch.Start();
            ShowMsg("启动监听");
        }

        public string get_my_ip()
        {
            string myIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    myIP = _IPAddress.ToString();
                }
            }
            
            return myIP;
        }

        public void Accept_connect()
        {
            while (true)
            {
                Socket sokConnection = socket_watch.Accept();
                var ssss = sokConnection.RemoteEndPoint.ToString().Split(':');
                ShowMsg("连接：" + ssss[0] + " :" + ssss[1]);
                
                if (listBox_online.FindString(ssss[0]) < 0)
                {
                    listBox_online.Items.Add(sokConnection.RemoteEndPoint.ToString());
                }
                
                Thread thr = new Thread(Get_msg);
                thr.IsBackground = true;
                thr.Start(sokConnection);
            }
        }

        void Get_msg(object sokConnectionparn)
        {
            Socket sokClient = sokConnectionparn as Socket;
            string client_ip = sokClient.RemoteEndPoint.ToString().Split(':')[0];
            
            while (true)
            {
                byte[] arrMsgRec = new byte[1024];
                int length = -1;
                try
                {
                    length = sokClient.Receive(arrMsgRec);
                    if (length > 0)
                    {
                        string msg = System.Text.Encoding.ASCII.GetString(arrMsgRec, 0, length);
                        ShowMsg("Receive:" + msg);
                        string[] msg_split = msg.Split('_');
                        if (msg_split.Length>1)
                        {
                            if(msg_split[1] == "net2019")
                            { 
                                sokClient.Send(Encoding.ASCII.GetBytes("lol"));
                                if (!ip_online.TryGetValue(msg_split[0], out string a))
                                {
                                    ip_online.Add(msg_split[0], client_ip);
                                }
                            }
                            else
                            {

                            }
                        }
                        else if(msg[0]=='q')
                        {
                            string user_name = msg.Substring(1);
                            string user_ip = "n";
                            ip_online.TryGetValue(user_name, out user_ip);
                            if(user_ip==null)
                            {
                                sokClient.Send(Encoding.ASCII.GetBytes("n"));
                            }
                            else
                            {
                                sokClient.Send(Encoding.ASCII.GetBytes(user_ip));
                            }
                            
                        }
                        else if(msg.Length>=6&&msg.Substring(0,6)=="logout")
                        {
                            ip_online.Remove(msg.Substring(6));
                        }
                  
                    }
                    else
                    {
                        listBox_online.Items.Remove(sokClient.RemoteEndPoint.ToString());
                        ShowMsg("" + sokClient.RemoteEndPoint.ToString() + "断开连接\r\n");
                        break;
                    }
                }
                catch (SocketException se)
                {
                    listBox_online.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    ShowMsg("" + sokClient.RemoteEndPoint.ToString() + "断开,异常消息：" + se.Message + "\r\n");
                    break;
                }
                catch (Exception e)
                {
                    listBox_online.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    ShowMsg("异常消息：" + e.Message + "\r\n");
                    break;
                }
            }
        }

        void ShowMsg(string str)
        {
            txtMsg.AppendText(str + "\r\n");
        }
    }
}
