using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private delegate void UpdateStatusCallback(string strMessage);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            IPAddress ipAddr = IPAddress.Parse(txtIp.Text);
            ChatServer mainServer = new ChatServer(ipAddr);
            ChatServer.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);
            mainServer.StartListening();
            txtLog.AppendText("Monitoring for connections...\r\n");
        }

        public void mainServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { e.EventMessage });
        }

        private void UpdateStatus(string strMessage)
        {
            txtLog.AppendText(strMessage + "\r\n");
        }

        private void txtIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
