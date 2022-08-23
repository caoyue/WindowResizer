using System;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Core.Shortcuts
{
    public static class KeyHelper
    {
        public static bool KeysEqual(this Hotkeys? keys, ModifierKeys modifier, Keys key)
        {
            return keys != null && modifier == keys.GetModifierKeys() && key == keys.GetKey();
        }

        public static ModifierKeys GetModifierKeys(this Hotkeys hotKeys)
        {
            ModifierKeys keys = 0;
            foreach (var k in hotKeys.ModifierKeys)
            {
                if (!Enum.TryParse(k, true, out ModifierKeys m))
                    continue;

                keys |= m;
            }

            return keys;
        }

        public static Keys GetKey(this Hotkeys hotKeys) =>
            Enum.TryParse(hotKeys.Key, true, out Keys k)
                ? k
                : throw new ArgumentException($"Key {hotKeys.Key} not valid.");

        public static bool IsModifierKey(this Keys key)
        {
            return key is Keys.Control or Keys.LControlKey or Keys.RControlKey
                or Keys.Shift or Keys.LShiftKey or Keys.RShiftKey
                or Keys.Alt or Keys.Menu or Keys.LMenu or Keys.RMenu
                or Keys.LWin or Keys.RWin;
        }

        public static string ToKeyString(this Keys key)
        {
            return key switch
            {
                Keys.Control or Keys.LControlKey or Keys.RControlKey => ModifierKeys.Ctrl.ToString(),
                Keys.Shift or Keys.LShiftKey or Keys.RShiftKey => ModifierKeys.Shift.ToString(),
                Keys.Alt or Keys.Alt or Keys.Menu or Keys.LMenu or Keys.RMenu => ModifierKeys.Alt.ToString(),
                Keys.LWin or Keys.RWin => ModifierKeys.Win.ToString(),
                _ => key.ToString()
            };
        }
    }
}
