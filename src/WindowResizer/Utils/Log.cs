using System;
using System.IO;

namespace WindowResizer.Utils
{
    public static class Log
    {
        private const string LogFileName = "WindowResizer.error.log";
        private const int LogFileSize = 1024 * 1024 * 10;
        private static string _logFile = LogFileName;

        public static void Append(string message)
        {
            File.AppendAllText(_logFile, $"[{DateTime.Now}]Error: {message}\n");

            var file = new FileInfo(_logFile);
            if (file.Length > LogFileSize)
            {
                try
                {
                    file.MoveTo($"{_logFile}-{DateTime.Now:yyyyMMdd}");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public static void SetLogPath(string logPath)
        {
            _logFile = Path.Combine(logPath, LogFileName);
        }
    }
}
