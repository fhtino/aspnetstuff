using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SoapLikeServer
{
    public static class SimpleLog
    {
        public static void WriteLine(string message)
        {
            string logFilePath = "c://temp//mylog.txt";
            string logEntry = $"{DateTime.UtcNow.ToString("O")} - {message}\n";
            File.AppendAllText(logFilePath, logEntry);
        }
    }
}