using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowResizer
{
    public static class App
    {
        public static Dictionary<KeyBindType, int> RegisteredHotKeys { get; } = new Dictionary<KeyBindType, int>();

        public static void ShowMessageBox(string message, MessageBoxIcon icon = MessageBoxIcon.Error, string title = nameof(WindowResizer))
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }

    public enum KeyBindType
    {
        Save,
        Restore,
        RestoreAll
    }
}
