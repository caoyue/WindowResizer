using System.Collections.Generic;
using WindowResizer.Configuration;

namespace WindowResizer
{
    public static class App
    {
        static App()
        {
            var helpers = new DesktopBridge.Helpers();
            IsRunningAsUwp = helpers.IsRunningAsUwp();
        }

        public const string DefaultFontFamilyName = "Tahoma";

        public static Dictionary<HotkeysType, int> RegisteredHotKeys { get; } = new Dictionary<HotkeysType, int>();

        public static bool IsRunningAsUwp { get; }
    }
}
