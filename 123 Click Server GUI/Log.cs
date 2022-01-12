using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _123_Click_Server_GUI
{
    static class Log
    {
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\r0p3\Log\";
        static public void addToLog(string message)
        {
            try
            {
                string fileName = folderPath + DateTime.Now.ToShortDateString() + ".txt";
                folderExistensCheck();
                if (File.Exists(fileName))
                    File.WriteAllText(fileName, File.ReadAllText(fileName) + message + Environment.NewLine);
                else
                    File.WriteAllText(fileName, message + Environment.NewLine);
                Form1.logBacklog.Add(message);
            }
            catch
            {
                addToLog(message);
            }
        }

        static private void folderExistensCheck()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
    }
}
