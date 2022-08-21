using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowResizer
{
    public static class App
    {
        public const string DefaultFontFamilyName = "Tahoma";

        public static Dictionary<KeyBindType, int> RegisteredHotKeys { get; } = new Dictionary<KeyBindType, int>();


    }

    public enum KeyBindType
    {
        Save,
        Restore,
        RestoreAll
    }
}
