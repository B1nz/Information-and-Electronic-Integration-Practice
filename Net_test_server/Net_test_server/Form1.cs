using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;

using System.Threading;

using System.IO;

namespace Net_test_server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string name = Dns.GetHostName();
            IPAddress[] IP_add = Dns.GetHostAddresses(name);
            label1.Text = "Server_IP=" + IP_add[1].ToString();
        }

        void showMsg(string str,int flag)
        {
            textBox1.AppendText(str + "\r\n");

            if (flag==1)
                pictureBox1.BackColor = Color.Green;
            else if(flag == 2)
                    pictureBox1.BackColor = Color.Red;            
        }

        Socket socketwatch;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //點選開始偵聽的時候，伺服器建立一個負責監聽IP地址跟埠號的Socket
                socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //建立埠物件
                IPEndPoint point = new IPEndPoint(ip, 8001);
                //繫結
                socketwatch.Bind(point);

                showMsg("監聽成功！",0);
                socketwatch.Listen(10);
               
                //建立一個執行緒
                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketwatch);

            }
            catch
            {
                showMsg("監聽失敗",0);
            }
        }

        Socket socketSend;
        //等待使用者端的連線
        void Listen(Object o)
        {
            try
            {
                Socket socketwatch = o as Socket;
                while (true)
                {
                    //等待使用者端的連線
                    socketSend = socketwatch.Accept();
                    showMsg(socketSend.RemoteEndPoint.ToString() + ":" + "連線成功！",0);
                    //開啟一個新的執行緒不斷的接收使用者端資訊
                    Thread th = new Thread(Receive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                }
            }
            catch
            { }
        }

        //接收使用者端傳送的資訊
        void Receive(Object o)
        {
            try
            {
                Socket socketSend = o as Socket;
                byte[] buffer = new byte[1024 * 1024 * 2];
                while (true)
                {                   
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    if (str.Equals("Hello"))
                        showMsg(socketSend.RemoteEndPoint + ":" + str, 1);
                    else
                        showMsg(socketSend.RemoteEndPoint + ":" + str, 2);

                }
            }
            catch
            { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox1.Clear();
            string str = textBox2.Text;
            //Console.WriteLine(str);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketSend.Send(buffer);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            socketwatch.Close();
            socketSend.Close();
        }
    }
}
