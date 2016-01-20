using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private string UserName = "Unknown";
        private StreamWriter swSender;
        private StreamReader srReceiver;
        private TcpClient tcpServer;

        private delegate void UpdateLogCallback(string strMessage);

        private delegate void CloseConnectionCallback(string strReason);

        private Thread thrMessaging;
        private IPAddress ipAddr;
        private bool Connected;

        public Form1()
        {
            //Register reg = new Register();
            //reg.ShowDialog();
            //tcpServer = reg.tcpServer;
            //swSender = reg.swSender;
            //srReceiver = reg.srReceiver;
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();

        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (Connected == true)
            {
                Connected = false;
                swSender.Close();
                srReceiver.Close();
                tcpServer.Close();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (Connected == false)
            {
                InitializeConnection();
            }
            else
            {
                CloseConnection("Disconnected at user's request.");
            }
        }

        private void InitializeConnection()
        {
           
                ipAddr = IPAddress.Parse(txtIp.Text);
                tcpServer = new TcpClient();
                tcpServer.Connect(ipAddr, 1986);
                
                Connected = true;
                UserName = txtUser.Text;

                txtIp.Enabled = false;
                txtUser.Enabled = false;
                txtMessage.Enabled = true;
                btnSend.Enabled = true;
                btnConnect.Text = "Отключение";
                string pass = txtPass.Text;
                swSender = new StreamWriter(tcpServer.GetStream());
                swSender.WriteLine("Auth|" + txtUser.Text + ":" + pass);
                swSender.Flush();
               
                thrMessaging = new Thread(new ThreadStart(ReceiveMessages));
                thrMessaging.Start();
            
        }

        private void ReceiveMessages()
        {
            srReceiver = new StreamReader(tcpServer.GetStream());
            string ConResponse = srReceiver.ReadLine();
            if (ConResponse.Contains("Успешн"))
            {
                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] {"Connected Successfully!"});
            }
            else 
            {
                string Reason = "Not Connected: ";
     
                Reason += ConResponse;
                this.Invoke(new CloseConnectionCallback(this.CloseConnection), new object[] { Reason });
                return;
            }
            while (Connected)
            {
                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] {srReceiver.ReadLine()});
            }
        }

        private void UpdateLog(string strMessage)
        {
            if (strMessage.Contains("Users"))
            {
                string[] d = strMessage.Split('|');
                listBox1.Items.Clear();
                for (int i = 1; i < d.Count(); i++)
                {
                    listBox1.Items.Add(d[i]);
                }
                return;
            }
            int my1stPosition = 0; 
            txtLog.AppendText(strMessage + "\r\n");
            if (strMessage.Contains(":"))
            {
               
                 my1stPosition = txtLog.Find(strMessage);
                txtLog.SelectionStart = my1stPosition;
                txtLog.SelectionLength = 9;
                txtLog.SelectionColor = Color.Indigo;
            }
            if (strMessage.Contains("|Server:"))
            {

                txtLog.SelectionStart = my1stPosition + 9;
                txtLog.SelectionLength = txtLog.TextLength;
                txtLog.SelectionColor = Color.Red;
            }
            else
            {
                txtLog.SelectionStart = my1stPosition + 9;
                txtLog.SelectionLength = txtLog.TextLength;
                txtLog.SelectionColor = Color.Blue;
            }
            
            // txtLog.Select(Text.IndexOf("\r\n"), 10);
           // txtLog.SelectionColor = Color.Red;
           
        }

        // Closes a current connection
        private void CloseConnection(string Reason)
        {   
            thrMessaging.Abort();
            txtLog.AppendText(Reason + "\r\n");

            txtIp.Enabled = true;
            txtUser.Enabled = true;
            txtMessage.Enabled = false;
            btnSend.Enabled = false;
            btnConnect.Text = "Connect";

            // Close the objects
            Connected = false;
            swSender.Close();
            srReceiver.Close();
            tcpServer.Close();
        }

        // Sends the message typed in to the server
        private void SendMessage()
        {
            if (txtMessage.Lines.Length >= 1)
            {
                swSender.WriteLine("All|"+txtMessage.Text);
                swSender.Flush();
                txtMessage.Lines = null;
            }
            txtMessage.Text = "";
        }

        // We want to send the message when the Send button is clicked
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // But we also want to send the message once Enter is pressed
        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) 13)
            {
                SendMessage();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new Register().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://pushfd.su/ExTime_SoftWare/api/config.php";



            using (var webClient = new WebClient())
            {

                // Создаём коллекцию параметров

                var pars = new NameValueCollection();



                // Добавляем необходимые параметры в виде пар ключ, значение

                pars.Add("skype", "MS10-rain.hackforums");
                pars.Add("hwid", "MS10-C39BB067858FAF5216807A42CFCA0562");
                pars.Add("software", "ms_software");

                // Посылаем параметры на сервер

                // Может быть ответ в виде массива байт

                var response = webClient.UploadValues(url, pars);

            }
        }
    }
}

