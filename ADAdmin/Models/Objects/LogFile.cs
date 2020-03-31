using System;
using System.IO;
using System.Threading;

namespace ADAdmin.Models.Objects
{
    public class LogFile
    {
        string path;
        public LogFile()
        {
            path = Properties.Resources.LogFilePath;
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Wystąpił bład przy tworzeniu pliku logu: " + e.Message);
            }
        }

        public void Write(string msg)
        {
            if (msg == null)
                msg = "-NULL-";
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(GetLogTime() + " - " + msg);
                    sw.AutoFlush = true;
                    sw.Close();
                }
            }
            catch (Exception)
            {
                //throw new Exception("Wystąpił bład przy zapisywaniu wiadomości do logu: " + e.Message);
                Thread.Sleep(1000);
                Write(msg);
            }
        }

        private string GetLogTime()
        {
            DateTime now = DateTime.Now;
            return IntToLazyZeroedString(now.Day, 2) + "/" + IntToLazyZeroedString(now.Month, 2) + "/" + IntToLazyZeroedString(now.Year, 4) + " " +
                    IntToLazyZeroedString(now.Hour, 2) + ":" + IntToLazyZeroedString(now.Minute, 2) + ":" + IntToLazyZeroedString(now.Second, 2);
        }

        private string IntToLazyZeroedString(int value, int decNumber)
        {
            string str = value.ToString();
            while (str.Length < decNumber)
                str = "0" + str.ToString();
            return str;
        }
    }
}
