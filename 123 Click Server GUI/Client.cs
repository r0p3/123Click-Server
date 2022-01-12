using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading.Tasks;

namespace _123_Click_Server_GUI
{
    public class Client
    {
        private const int PINGTIME = 60000;

        public Socket socket;
        private byte[] buffer = new byte[1024];
        private string updatePath = @"C:\Program Files (x86)\r0p3\123 Click";
        public string name = "";
        public string IP = "";
        public List<string> filesToUpdate = new List<string>();

        public event EventHandler onUserListChange;
        private Form1 form1;
        private System.Timers.Timer timer;
        private bool updating = false;

        public Client(Socket socket, Form1 form1)
        {
            this.socket = socket;
            this.form1 = form1;
            this.IP = socket.RemoteEndPoint.ToString().Split(':').First();
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onBeginReceive, null);
            timer = new System.Timers.Timer(PINGTIME);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Task.Delay(new TimeSpan(0, 0, 10)).ContinueWith(o => { checkIfName(); });
        }

        private void checkIfName()
        {
            if(name == "" && !updating)
            {
                IPBan.Ban(IP, "No name after 10 seconds");
                disconnectClient("No name");
                form1.removeMe(this);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(name != "")
                sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Ping, ""));
        }

        private void onBeginReceive(IAsyncResult ar)
        {
            try
            {
                int size = socket.EndReceive(ar);
                handleMessage(Encoding.UTF8.GetString(buffer, 0, size));
            }
            catch (SocketException)
            {
                form1.removeMe(this);
            }
        }

        private void onBeginSend(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch
            {

            }
        }

        private void handleMessage(string message)
        {
            MessageProtocol.MessageType messageType = MessageProtocol.getMessageType(message);
            string content = MessageProtocol.getMessage(message);

            logUpdate(message);

            switch (messageType)
            {
                case MessageProtocol.MessageType.Name:
                    name = content;
                    sendToAll(MessageProtocol.createMessage(MessageProtocol.MessageType.OnlineUsers, getAllUsers()));
                    onUserListChange?.Invoke(this, null);
                    form1.updateOnline();
                    break;
                case MessageProtocol.MessageType.StartCountDown:
                    sendToAll(Encoding.UTF8.GetBytes(message));
                    break;
                case MessageProtocol.MessageType.CancelCountDown:
                    sendToAll(Encoding.UTF8.GetBytes(message));
                    break;
                case MessageProtocol.MessageType.Rewind:
                    sendToAll(Encoding.UTF8.GetBytes(message));
                    break;
                case MessageProtocol.MessageType.Forward:
                    sendToAll(Encoding.UTF8.GetBytes(message));
                    break;
                case MessageProtocol.MessageType.Disconnect:
                    form1.removeMe(this);
                    sendToAll(MessageProtocol.createMessage(MessageProtocol.MessageType.OnlineUsers, getAllUsers()));
                    break;
                case MessageProtocol.MessageType.Version:
                    try
                    {
                        if (int.Parse(content) < Properties.Settings.Default.Version)
                            sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Update, ""));
                    }
                    catch { }
                    break;
                case MessageProtocol.MessageType.Update:
                    filesToUpdate = Directory.GetFiles(updatePath).ToList();
                    specialSendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Update, ""));
                    updating = true;
                    break;
                case MessageProtocol.MessageType.NextFile:
                    if (filesToUpdate.Count != 0)
                    {
                        if (filesToUpdate.First().Replace(updatePath + "\\", "") == "123Updater.exe")
                            sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.FileName, "tempUpdater.exe"));
                        else
                            sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.FileName, filesToUpdate.First().Replace(updatePath + "\\", "")));
                    }
                    else
                        sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Version, Properties.Settings.Default.Version.ToString()));
                    break;
                case MessageProtocol.MessageType.SendFile:
                    sendMessage(File.ReadAllBytes(filesToUpdate.First()));
                    filesToUpdate.RemoveAt(0);
                    break;
                case MessageProtocol.MessageType.None:
                    if (message.Length > 1)
                    {
                        IPBan.Ban(socket.RemoteEndPoint.ToString().Split(':').First(), message);
                        form1.removeMe(this);
                    }
                    break;
            }
            if (messageType != MessageProtocol.MessageType.None && messageType != MessageProtocol.MessageType.Disconnect)
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onBeginReceive, null);
        }

        public void sendMessage(byte[] message)
        {
            try
            {
                byte[] buffer = message;
                socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, onBeginSend, this);
            }
            catch
            {

            }
        }

        public void specialSendMessage(byte[] message)
        {
            byte[] buffer = message;
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, onBeginSend, this);
        }

        private void sendToAll(byte[] message)
        {
            foreach (var item in Form1.clients)
            {
                if(item.name != "")
                    item.sendMessage(message);
            }
        }

        public string getAllUsers()
        {
            string userNames = "";
            foreach (var item in Form1.clients)
            {
                userNames += item.name + ",";
            }
            return userNames.TrimEnd(',');
        }

        public void disconnectClient(string message)
        {
            if (name != "")
                sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Disconnect, message));
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch { }
        }

        private void logUpdate(string message)
        {
            switch (MessageProtocol.getMessageType(message))
            {
                case MessageProtocol.MessageType.Name:
                case MessageProtocol.MessageType.StartCountDown:
                case MessageProtocol.MessageType.CancelCountDown:
                case MessageProtocol.MessageType.Disconnect:
                case MessageProtocol.MessageType.Version:
                case MessageProtocol.MessageType.Update:
                case MessageProtocol.MessageType.None:
                case MessageProtocol.MessageType.Rewind:
                    Log.addToLog("Time: " + DateTime.Now.ToLongTimeString());
                    Log.addToLog("IP: " + socket.RemoteEndPoint.ToString().Split(':').First());
                    Log.addToLog("Name: " + name);
                    Log.addToLog("Message: " + message);
                    Log.addToLog("");
                    break;
                default:
                    break;
            }

        }
    }
}
