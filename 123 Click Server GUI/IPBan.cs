using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _123_Click_Server_GUI
{
    class IPBan
    {
        private static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\r0p3\Banned-IP.txt";
        public static void Ban(string IP, string reason)
        {
            if (!isBanned(IP))
            {
                checkFileExist();
                DateTime dateTime = DateTime.Now;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);
                XmlNode root = xmlDocument.SelectSingleNode("IP_Bans");
                XmlElement ipBanned = xmlDocument.CreateElement("IP_Ban");
                root.AppendChild(ipBanned);

                XmlElement xmlIP = xmlDocument.CreateElement("IP");
                xmlIP.InnerText = IP;
                ipBanned.AppendChild(xmlIP);

                XmlElement xmlTime = xmlDocument.CreateElement("Time");
                xmlTime.InnerText = dateTime.ToLongTimeString();
                ipBanned.AppendChild(xmlTime);

                XmlElement xmlDate = xmlDocument.CreateElement("Date");
                xmlDate.InnerText = dateTime.ToShortDateString();
                ipBanned.AppendChild(xmlDate);

                XmlElement xmlReason = xmlDocument.CreateElement("Reason");
                xmlReason.InnerText = reason;
                ipBanned.AppendChild(xmlReason);

                xmlDocument.Save(filePath);
                addToLog(IP, reason, dateTime);
            }
        }

        public static bool isBanned(string IP)
        {
            checkFileExist();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            return (xmlDocument.SelectSingleNode("IP_Bans/IP_Ban[IP='" + IP + "']") != null);
        }

        private static void checkFileExist()
        {
            if (!File.Exists(filePath) || File.ReadAllBytes(filePath).Count() == 0)
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement root = xmlDocument.CreateElement("IP_Bans");
                xmlDocument.AppendChild(root);
                xmlDocument.Save(filePath);
            }
        }

        private static void addToLog(string IP, string reason, DateTime dateTime)
        {
            Log.addToLog("IP-Ban");
            Log.addToLog(IP);
            Log.addToLog(reason);
            Log.addToLog(dateTime.ToLongTimeString());
            Log.addToLog(dateTime.ToShortDateString());
            Log.addToLog("");
        }
    }
}
