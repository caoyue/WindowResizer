using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowResizer.Utils
{
    public static class Startup
    {
        private const string RegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static bool StartupStatus()
        {
            var key = Registry.CurrentUser.OpenSubKey(RegPath, true);

            var p = key?.GetValue(nameof(WindowResizer));
            return p != null;
        }

        public static void AddToStartup()
        {
            var key = Registry.CurrentUser.OpenSubKey(RegPath, true);
            key?.SetValue(nameof(WindowResizer), Application.ExecutablePath);
        }

        public static void RemoveFromStartup()
        {
            var key = Registry.CurrentUser.OpenSubKey(RegPath, true);
            key?.DeleteValue(nameof(WindowResizer), false);
        }
    }
}
