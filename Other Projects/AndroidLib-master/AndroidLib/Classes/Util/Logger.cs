using System;
using System.IO;

namespace RegawMOD
{
    internal static class Logger
    {
        private static string ErrorLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("AndroidLib", "ErrorLog.txt"));

        internal static bool WriteLog(string Message, string Title, string StackTrace)
        {
            try
            {
                using (FileStream fs = new FileStream(ErrorLogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.WriteLine(String.Join(" ", new string[] { Title, Message, StackTrace }));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}