using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _123_Click_Server_GUI
{
    static class MessageProtocol
    {
        private static char SPLITTER = '|';
        public static MessageType getMessageType(string message)
        {
            try
            {
                MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), message.Split(SPLITTER).First());
                return messageType;
            }
            catch
            {
                return MessageType.None;
            }
        }

        public static string getMessage(string message)
        {
            return message.Split(SPLITTER).Last();
        }

        public static byte[] createMessage(MessageType messageType, string message)
        {
            return Encoding.UTF8.GetBytes(messageType.ToString() + SPLITTER + message);
        }

        public enum MessageType
        {
            Name,
            StartCountDown,
            CancelCountDown,
            Rewind,
            Forward,
            Disconnect,
            OnlineUsers,
            Version,
            Update,
            NextFile,
            SendFile,
            FileName,
            ForceUpdate,
            Ping,
            None
        }
    }
}
