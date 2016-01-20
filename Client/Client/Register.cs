using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        public StreamWriter swSender;
        public StreamReader srReceiver;
        public TcpClient tcpServer; 
        private IPAddress ipAddr;
        private void button1_Click(object sender, EventArgs e)
        {
            ipAddr = IPAddress.Parse(txtIp.Text);
            tcpServer = new TcpClient();
            tcpServer.Connect(ipAddr, 1986);

            string user = txtUser.Text;
            string pass = txtPass.Text;
            txtIp.Enabled = false;
            swSender = new StreamWriter(tcpServer.GetStream());
            swSender.WriteLine("Auth|" + txtUser.Text + ":" + pass);
            swSender.Flush();
            ReceiveMessages();
            
        }
        private void ReceiveMessages()
        {
            srReceiver = new StreamReader(tcpServer.GetStream());
            string ConResponse = srReceiver.ReadLine();
            MessageBox.Show(ConResponse);
            swSender.Close();
            srReceiver.Close();
            tcpServer.Close();
            Close();
           // new Form1(tcpServer, srReceiver, swSender).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ipAddr = IPAddress.Parse(txtIp.Text);
            tcpServer = new TcpClient();
            tcpServer.Connect(ipAddr, 1986);

            string user = txtUser.Text;
            string pass = txtPass.Text;
            txtIp.Enabled = false;
            swSender = new StreamWriter(tcpServer.GetStream());
            swSender.WriteLine("Reg|"+txtUser.Text+":"+pass);
            swSender.Flush();
            ReceiveMessages();
            
        }
    }
}
