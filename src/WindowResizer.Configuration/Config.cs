using System;
using System.Collections.Generic;
using System.ComponentModel;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Configuration
{
    public class Config
    {
        public bool DisableInFullScreen { get; set; } = true;

        public bool CheckUpdate { get; set; } = true;

        public Dictionary<HotkeysType, Hotkeys> Keys { get; set; } = new();


        public BindingList<WindowSize> WindowSizes { get; set; } = new();

        [Obsolete]
        public Hotkeys? SaveKey { internal get; set; }

        [Obsolete]
        public Hotkeys? RestoreKey { internal get; set; }

        [Obsolete]
        public Hotkeys? RestoreAllKey { internal get; set; }

        public static Dictionary<HotkeysType, Hotkeys> DefaultKeys => new()
        {
            {
                HotkeysType.Save, new()
                {
                    ModifierKeys = new HashSet<string>
                    {
                        "Ctrl", "Alt"
                    },
                    Key = "S"
                }
            },
            {
                HotkeysType.Restore, new()
                {
                    ModifierKeys = new HashSet<string>
                    {
                        "Ctrl", "Alt"
                    },
                    Key = "R"
                }
            }
        };
    }
}
