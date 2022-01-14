using System;
using System.Windows.Forms;
using WindowResizer.Core;
using WindowResizer.Core.KeyboardHook;

namespace WindowResizer.Configuration
{
    public static class ConfigHelper
    {
        public static ModifierKeys GetModifierKeys(this HotKeys hotKeys)
        {
            ModifierKeys keys = 0;
            foreach (var k in hotKeys.ModifierKeys)
            {
                ModifierKeys m;
                if (!Enum.TryParse(k, true, out m))
                    continue;

                keys |= m;
            }

            return keys;
        }

        public static Keys GetKey(this HotKeys hotKeys) =>
            Enum.TryParse(hotKeys.Key, true, out Keys k) ? k : new HotKeys().GetKey();

        public static bool ValidateKeys(this HotKeys hotKeys) =>
            !(hotKeys.ModifierKeys == null ||
                hotKeys.ModifierKeys.Length == 0 ||
                string.IsNullOrEmpty(hotKeys.Key));

        public static string ToKeysString(this HotKeys hotKeys)
        {
            string str = string.Empty;
            if (hotKeys.ModifierKeys != null && hotKeys.ModifierKeys.Length > 0)
            {
                str += string.Join(" + ", hotKeys.ModifierKeys);
            }

            return $"{str} + {hotKeys.Key}";
        }
    }
}
