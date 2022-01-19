using System;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Core.Shortcuts
{
    public class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            Modifier = modifier;
            Key = key;
        }

        public ModifierKeys Modifier { get; }

        public Keys Key { get; }
    }
}
