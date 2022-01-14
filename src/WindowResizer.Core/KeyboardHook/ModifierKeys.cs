﻿using System;

namespace WindowResizer.Core.KeyboardHook
{
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        Win = 8
    }
}
