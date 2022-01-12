using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _123_Click_Server_GUI
{
    public partial class Form1 : Form
    {
        public static ObservableCollection<Client> clients = new ObservableCollection<Client>();
        private static Socket socket;
        public static ObservableCollection<string> logBacklog = new ObservableCollection<string>();

        public Form1()
        {
            InitializeComponent();
            logBacklog.CollectionChanged += LogBacklog_CollectionChanged;
            clients.CollectionChanged += Clients_CollectionChanged;
            startServer();
        }

        #region Socket
        private void startServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            socket.Listen(10);
            socket.BeginAccept(onBeginAccept, null);
        }

        private void onBeginAccept(IAsyncResult ar)
        {
            Socket client = socket.EndAccept(ar);

            if (!IPBan.isBanned(client.RemoteEndPoint.ToString().Split(':').First()))
            {
                clients.Add(new Client(client, this));
                clients.Last().onUserListChange += Form1_onUserListChange;
            }
            else
            {
                Log.addToLog("Banned connection");
                Log.addToLog("IP: " + client.RemoteEndPoint.ToString().Split(':').First());
                Log.addToLog("Time: " + DateTime.Now.ToLongTimeString());
                Log.addToLog("");
                client.Shutdown(SocketShutdown.Both);
            }

            socket.BeginAccept(onBeginAccept, null);
        }

        private void Form1_onUserListChange(object sender, EventArgs e)
        {
            updateOnline();
        }

        public void removeMe(Client connected)
        {
            try
            {
                connected.socket.Shutdown(SocketShutdown.Both);
                connected.socket.Close();
            }
            catch { }
            connected.onUserListChange -= Form1_onUserListChange;
            clients.Remove(connected);
            if(clients.Count > 0)
            foreach (var item in clients)
                    if(item.name != "")
                        item.sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.OnlineUsers, item.getAllUsers()));
            updateOnline();
        }
        #endregion

        #region GUI
        private void LogBacklog_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(logBacklog.Count > 0)
                BeginInvoke(new MethodInvoker(delegate
                {
                    string date = "[" + DateTime.Now.ToLongTimeString() + "] ";
                    if(logBacklog.Count > 0)
                    {
                        if (logBacklog.First().Length > 0)
                            rbLog.AppendText(date + logBacklog.First() + Environment.NewLine);
                        else
                            rbLog.AppendText(Environment.NewLine);
                        logBacklog.RemoveAt(0);
                        rbLog.ScrollToCaret();
                    }
                    
                }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nudVersion.Value = Properties.Settings.Default.Version;
            nudVersion.ValueChanged += nudVersion_ValueChanged;
        }

        private void Clients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateOnline();
        }

        public void updateOnline()
        {
                BeginInvoke(new MethodInvoker(delegate
                {
                    try
                    {
                        lvOnline.Items.Clear();
                        if (clients.Count > 0)
                            foreach (var item in clients)
                            {
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = item.name;
                                lvi.SubItems.Add(item.IP);
                                lvOnline.Items.Add(lvi);
                            }
                        bool modifier = lvOnline.Items.Count > 0 && lvOnline.SelectedItems.Count > 0;
                        btnBan.Enabled = modifier;
                        btnKick.Enabled = modifier;
                    }
                    catch
                    {
                        updateOnline();
                    }
                }));
        }
        #endregion

        private void btnBan_Click(object sender, EventArgs e)
        {
            if(lvOnline.SelectedItems.Count > 0)
            {
                foreach (var item in clients)
                    if (item.name == lvOnline.SelectedItems[0].Text)
                    {
                        item.sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Disconnect, ""));
                        IPBan.Ban(item.IP, "Manual ban");
                        logBacklog.Add("Banned " + item.name + ", Manual ban");
                        logBacklog.Add("");
                    }
            }
        }

        private void btnKick_Click(object sender, EventArgs e)
        {
            if (lvOnline.SelectedItems.Count > 0)
            {
                foreach (var item in clients)
                    if (item.name == lvOnline.SelectedItems[0].Text)
                    {
                        item.sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Disconnect, ""));
                        logBacklog.Add("Kicked " + item.name);
                        logBacklog.Add("");
                    }
            }
        }

        private void lvOnline_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool modifier = lvOnline.Items.Count > 0 && lvOnline.SelectedItems.Count > 0;
            btnBan.Enabled = modifier;
            btnKick.Enabled = modifier;
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

            }
        }

        public void consoleCommands()
        {
            bool running = true;

            while (running)
            {
                string stringCommand = Console.ReadLine();
                string paramater = "";
                if (stringCommand.Contains(" "))
                    paramater = stringCommand.Split(' ')[1];
                try
                {
                    commands command = (commands)Enum.Parse(typeof(commands), stringCommand.Split(' ').First());
                    {
                        switch (command)
                        {
                            case commands.help:
                                break;
                            case commands.version:
                                if (paramater != "")
                                {
                                    switch (paramater)
                                    {
                                        case "add":
                                            Properties.Settings.Default.Version = Properties.Settings.Default.Version + 1;
                                            if (clients.Count > 0)
                                            {
                                                foreach (var item in clients)
                                                {
                                                    item.sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.Update, Properties.Settings.Default.Version.ToString()));
                                                }
                                            }
                                            break;
                                        case "remove":
                                            Properties.Settings.Default.Version = Properties.Settings.Default.Version - 1;
                                            break;
                                        default:
                                            break;
                                    }
                                    Properties.Settings.Default.Save();
                                }
                                logBacklog.Add("Version: " + Properties.Settings.Default.Version.ToString());
                                logBacklog.Add("");
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch
                {
                    logBacklog.Add("Could not find command: " + stringCommand);
                    logBacklog.Add("");
                }
            }
        }
        private enum commands
        {
            help,
            version,
            kick,
            ban,
            update,
            users
        }

        private void nudVersion_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Version = (int)nudVersion.Value;
            Properties.Settings.Default.Save();
            foreach (var item in clients)
            {
                item.sendMessage(MessageProtocol.createMessage(MessageProtocol.MessageType.ForceUpdate, ""));
            }
        }
    }
}
