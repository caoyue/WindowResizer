using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Core.Shortcuts
{
    public static class KeyHelper
    {
        public static ModifierKeys GetModifierKeys(this HotKeys hotKeys)
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

        public static Keys GetKey(this HotKeys hotKeys) =>
            Enum.TryParse(hotKeys.Key, true, out Keys k) ? k : new HotKeys().GetKey();

        public static bool ValidateKeys(this HotKeys hotKeys) =>
            hotKeys.ModifierKeys.Length > 0 && !string.IsNullOrEmpty(hotKeys.Key);

        public static List<string> ToKeys(this HotKeys hotKeys)
        {
            var list = new List<string>();

            if (hotKeys.Key is not null)
            {
                list.AddRange(hotKeys.ModifierKeys);
                list.Add(hotKeys.Key);
            }

            return list;
        }

        public static string ToKeysString(this HotKeys hotKeys)
        {
            var str = string.Empty;
            if (hotKeys.ModifierKeys.Length > 0)
            {
                str += string.Join(" + ", hotKeys.ModifierKeys);
            }

            return $"{str} + {hotKeys.Key}";
        }
    }
}
