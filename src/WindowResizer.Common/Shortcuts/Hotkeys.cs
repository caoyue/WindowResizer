using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowResizer.Common.Shortcuts;

public class Hotkeys
{
    public HashSet<string> ModifierKeys { get; set; } = new();

    public string? Key { get; set; }

    public void Clear()
    {
        ModifierKeys.Clear();
        Key = null;
    }

    public string ToKeysString() =>
        string.Join(" + ", GetAllKeys());

    public bool IsValid() =>
        this.ModifierKeys.Count > 0 && !string.IsNullOrEmpty(this.Key);

    public override bool Equals(object? obj) =>
        obj is Hotkeys other && this.GetAllKeys().SequenceEqual(other.GetAllKeys());

    public override int GetHashCode() =>
        ToKeysString().GetHashCode();

    private IEnumerable<string> GetAllKeys()
    {
        var keys = new List<string>();
        if (ModifierKeys.Any())
        {
            keys.AddRange(ModifierKeys);
        }

        if (!string.IsNullOrEmpty(Key))
        {
            keys.Add(Key!);
        }

        int CompareKeys(string a, string b)
        {
            var aMod = Enum.TryParse(a, out ModifierKeys aVal);
            var bMod = Enum.TryParse(b, out ModifierKeys bVal);
            return (aMod, bMod) switch
            {
                (true, true) => aVal.CompareTo(bVal),
                (true, false) => -1,
                (false, true) => 1,
                (false, false) => String.Compare(a, b, StringComparison.Ordinal),
            };
        }

        keys.Sort(CompareKeys);
        return keys;
    }
}