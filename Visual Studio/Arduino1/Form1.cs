using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino1
{
    public partial class Form1 : Form
    {
        double Vo_value = 0.0;
        double Vi_value = 0.0;
        double R_value = 20000;
        double C_value = 20 * 0.000001;
        double T_value = 0.001;
        double err_ = 0.0;
        double err_old_ = 0.0;
        double u_value = 0.0;

        double kp = 19.0;
        double ki = 50.0;
        double V_cmd = 1.0;

        UInt32 k = 0;
        int select_mode = 0;

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            select_mode = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button1.Text = "Start (Reset)";
            k = 0;
            Vo_value = 0.0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (select_mode == 0)
                Vi_value = V_cmd;
            else if (select_mode == 1)
            {
                err_ = V_cmd - Vo_value;
                u_value = u_value + kp * (err_ - err_old_) + ki * T_value * err_;
                Vi_value = 0;
                err_old_ = err_;
            }
            Vo_value = Vo_value + T_value / C_value / R_value * (-Vo_value + Vi_value);
            chart1.Series[0].Points.AddXY(k * T_value, V_cmd);
            chart1.Series[1].Points.AddXY(k * T_value, Vo_value);
            chart2.Series[0].Points.AddXY(k * T_value, Vi_value);
            label2.Text = (Vi_value - Vo_value).ToString("0.000");
            k = k + 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            select_mode = 1;
        }
    }
}
