using System.ComponentModel;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Configuration
{
    public class Config
    {
        public bool DisableInFullScreen { get; set; } = true;

        public HotKeys SaveKey { get; set; } = new() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "S" };

        public HotKeys RestoreKey { get; set; } = new() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "R" };

        public HotKeys RestoreAllKey { get; set; } = new() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "T" };

        public BindingList<WindowSize> WindowSizes { get; set; } = new();
    }
}
