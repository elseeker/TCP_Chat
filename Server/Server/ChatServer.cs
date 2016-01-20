using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class StatusChangedEventArgs : EventArgs
    {
        private string EventMsg;
        public string EventMessage
        {
            get
            {
                return EventMsg;
            }
            set
            {
                EventMsg = value;
            }
        }
        public StatusChangedEventArgs(string strEventMsg)
        {
            EventMsg = strEventMsg;
        }
    }
    public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

    class ChatServer
    {
        public static Hashtable htUsers = new Hashtable(30); 
        public static Hashtable htConnections = new Hashtable(30); 
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;
        public ChatServer(IPAddress address)
        {
            ipAddress = address;
        }
        private Thread thrListener;
        private TcpListener tlsClient;
        bool ServRunning = false;
        public static void AddUser(TcpClient tcpUser, string strUsername)
        {
            ChatServer.htUsers.Add(strUsername, tcpUser);
            ChatServer.htConnections.Add(tcpUser, strUsername);

            SendAdminMessage(htConnections[tcpUser] + " Подключился");
        }
        public static void RemoveUser(TcpClient tcpUser)
        {
            if (htConnections[tcpUser] != null)
            {
                SendAdminMessage(htConnections[tcpUser] + " Покинул чат");
                ChatServer.htUsers.Remove(ChatServer.htConnections[tcpUser]);
                ChatServer.htConnections.Remove(tcpUser);
            }
        }
        public static void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChangedEventHandler statusHandler = StatusChanged;
            if (statusHandler != null)
            {
                statusHandler(null, e);
            }
        }
        public static void SendAdminMessage(string Message)
        {
            StreamWriter swSenderSender;
            e = new StatusChangedEventArgs(GetTime() + "|" + "Server: " + Message);
            OnStatusChanged(e);
           TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            for (int i = 0; i < tcpClients.Length; i++)
            {
                try
                {
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                     {
                        swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                        swSenderSender.WriteLine(GetTime() + "|" + "Server: " + Message);
                        swSenderSender.Flush();
                        swSenderSender = null;
                    }
                }
                catch 
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        }

        public static string GetTime()
        {
            return DateTime.Now.ToLongTimeString();
        }
        public static void SendMessage(string From, string Message)
        {
            StreamWriter swSenderSender;
            string[] To = Message.Split('|');
            e = new StatusChangedEventArgs(GetTime()+"|"+From + " : " + Message);
            OnStatusChanged(e);
            string[] StringMessage = new string[ChatServer.htUsers.Count];
            ChatServer.htConnections.Values.CopyTo(StringMessage, 0);
          
            TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            for (int i = 0; i < tcpClients.Length; i++)
            {
                try
                {
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    if (Message.Contains("GetUsers"))
                    {
                        swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                        swSenderSender.WriteLine("Users|" + String.Join("|", StringMessage));
                        swSenderSender.Flush();
                        swSenderSender = null;
                       
                    }
                    else
                    if (To[0] == "All" || To[0].ToLower().Trim() == StringMessage[i].ToLower().Trim() || From.ToLower().Trim() == StringMessage[i].ToLower().Trim())
                    {
                        swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                        swSenderSender.WriteLine(GetTime() + "|" + From + ": " + To[1]);
                        swSenderSender.Flush();
                        swSenderSender = null;
                    }
                }
                catch 
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        }

        public void StartListening()
        {

            IPAddress ipaLocal = ipAddress;

            tlsClient = new TcpListener(1986);

            tlsClient.Start();

            ServRunning = true;

            thrListener = new Thread(KeepListening);
            thrListener.Start();
        }

        private void KeepListening()
        {
            while (ServRunning == true)
            {
                tcpClient = tlsClient.AcceptTcpClient();
                Connection newConnection = new Connection(tcpClient);
            }
        }
    }
    class Connection
    {
        TcpClient tcpClient;
        private Thread thrSender;
        private StreamReader srReceiver;
        private StreamWriter swSender;
        private string currUser;
        private string strResponse;
        public Connection(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
            thrSender = new Thread(AcceptClient);
            thrSender.Start();
        }

        private void CloseConnection()
        {
            tcpClient.Close();
            srReceiver.Close();
            swSender.Close();
        }

        private void RegistrationUser(string[] d)
        {
            string user = d[1].Split(':')[0];
            string pass = d[1].Split(':')[1];

            if (!File.Exists("Users.txt"))
            {
                File.AppendAllText("Users.txt", d[1]);
                swSender.WriteLine("Вы успешно зарегистрировались!");
                swSender.Flush();
            }
            else
            {
                string[] s = File.ReadAllLines("Users.txt");
                if (s.Count() > 0)
                    foreach (var VARIABLE in s)
                    {
                        if (!VARIABLE.Split(':')[0].Contains(user))
                        {
                            File.AppendAllText("Users.txt", user+":" +pass+ "\r\n");
                            swSender.WriteLine("Вы успешно зарегистрировались!");
                            swSender.Flush();
                        }
                        else
                        {
                            swSender.WriteLine("Пользователь с таким именем уже зарегистрирован!");
                            swSender.Flush();
                            CloseConnection();
                        }
                    }
                else
                {
                    File.AppendAllText("Users.txt", d + "\r\n");
                    swSender.WriteLine("Вы успешно зарегистрировались!");
                    swSender.Flush();
                }
            }
        }
        private void AcceptClient()
        {
            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());
            swSender = new System.IO.StreamWriter(tcpClient.GetStream());

            currUser = srReceiver.ReadLine();
            string[] d = currUser.Split('|');
            string user = d[1].Split(':')[0];
            if (d[0]=="Reg")
            {
               RegistrationUser(d);
               return;
            }
            else
                if (currUser.Contains("Auth|") && currUser!="")
            {
              
              
                string pass = d[1].Split(':')[1];
                bool auth = false;

                if (ChatServer.htUsers.Contains(user))
                {
                    swSender.WriteLine("Пользователь с таким именем уже в чате!");
                    swSender.Flush();
                    CloseConnection();
                    return;
                }
                else
                {
                    string[] s = File.ReadAllLines("Users.txt");
                    foreach (var VARIABLE in s)
                    {
                        if (VARIABLE == (d[1]))
                        {

                            swSender.WriteLine("Успешно авторизированы!");
                            swSender.Flush();
                            auth = true;
                            ChatServer.AddUser(tcpClient, user);

                            break;
                        }
                    }
                    if (!auth)
                    {
                        swSender.WriteLine("Ошибка авторизации!");
                        swSender.Flush();
                        CloseConnection();
                    }
                }
            }
            else
            {
                CloseConnection();
                return;
            }

            try
            {
                while ((strResponse = srReceiver.ReadLine()) != "")
                {
                    if (strResponse == null)
                    {
                        ChatServer.RemoveUser(tcpClient);
                    }
                    else
                    {
                        ChatServer.SendMessage(user, strResponse);
                    }
                }
            }
            catch
            {
                ChatServer.RemoveUser(tcpClient);
            }
        }
    }
}
