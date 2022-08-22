using System.Collections.Generic;
using System.ComponentModel;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Configuration
{
    public class Config
    {
        public bool DisableInFullScreen { get; set; } = true;

        public bool CheckUpdate { get; set; } = true;

        public Hotkeys SaveKey { get; set; } = new();

        public Hotkeys RestoreKey { get; set; } = new();

        public Hotkeys RestoreAllKey { get; set; } = new();

        public BindingList<WindowSize> WindowSizes { get; set; } = new();

        public static Hotkeys DefaultSaveKey => new()
        {
            ModifierKeys = new HashSet<string>
            {
                "Ctrl", "Alt"
            },
            Key = "S"
        };

        public static Hotkeys DefaultRestoreKey => new()
        {
            ModifierKeys = new HashSet<string>
            {
                "Ctrl", "Alt"
            },
            Key = "R"
        };

        public static Hotkeys DefaultRestoreAllKey => new()
        {
            ModifierKeys = new HashSet<string>
            {
                "Ctrl", "Alt"
            },
            Key = "T"
        };
    }
}
