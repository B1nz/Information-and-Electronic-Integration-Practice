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

namespace Net_test_client
{
    public partial class Form1 : Form
    {
        String ip_Add;

        public Form1()
        {
            InitializeComponent();
            string name = Dns.GetHostName();
            IPAddress[] IP_add = Dns.GetHostAddresses(name);
            label1.Text = "Client_IP=" + IP_add[1].ToString();
        }

        void showMsg(string s)
        {
            textBox1.AppendText(s);
        }

        Socket socket0;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Socket
                socket0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip_Add), 8001);
                socket0.Connect(point);

                showMsg("連線成功!"+"\r\n");

                Thread th = new Thread(Receive);
                th.IsBackground = true;
                th.Start();
            }
            catch(Exception)
            {
                showMsg("連線失敗!" + "\r\n");
            }
            
        }

        //使用者端接收伺服器傳送的訊息
        void Receive()
        {
            try
            {
                while (true)
                {                  
                    // Using Buffer
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socket0.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    showMsg(socket0.RemoteEndPoint + ":" + str+"\r\n");
                }
            }
            catch
            {
                showMsg("Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;

                string str1 = str;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str1);
                socket0.Send(buffer);            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ip_Add = comboBox1.SelectedItem.ToString();
        }
    }
}
