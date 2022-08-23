using System.Collections.Generic;
using WindowResizer.Configuration;

namespace WindowResizer
{
    public static class App
    {
        public const string DefaultFontFamilyName = "Tahoma";

        public static Dictionary<HotkeysType, int> RegisteredHotKeys { get; } = new Dictionary<HotkeysType, int>();
    }
}
