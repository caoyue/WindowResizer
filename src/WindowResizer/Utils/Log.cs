using System;
using System.IO;

namespace WindowResizer.Utils
{
    public static class Log
    {
        private const string LogFileName = "WindowResizer.error.log";
        private const int LogFileSize = 1024 * 1024 * 10;

        public static void Append(string message)
        {
            File.AppendAllText(LogFileName, $"[{DateTime.Now}]Error: {message}\n");

            var file = new FileInfo(LogFileName);
            if (file.Length > LogFileSize)
            {
                try
                {
                    file.MoveTo($"{LogFileName}-{DateTime.Now:yyyyMMdd}");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
