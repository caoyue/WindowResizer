using System.ComponentModel;
using WindowResizer.Core;

namespace WindowResizer.Configuration
{
    public class Config
    {
        public bool DisableInFullScreen { get; set; } = true;

        public HotKeys SaveKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "S" };

        public HotKeys RestoreKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "R" };

        public HotKeys RestoreAllKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "T" };

        public BindingList<WindowSize> WindowSizes { get; set; }
    }
}
