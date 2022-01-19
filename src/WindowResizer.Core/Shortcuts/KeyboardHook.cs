using System;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;
using static WindowResizer.Core.Shortcuts.NativeMethods;

namespace WindowResizer.Core.Shortcuts
{

    public sealed class KeyboardHook : IDisposable
    {
        private readonly Window _window = new();
        private int _currentId;

        public KeyboardHook()
        {
            KeyHook();
        }

        private void KeyHook()
        {
            _window.KeyPressed += delegate (object? _, KeyPressedEventArgs args)
            {
                KeyPressed?.Invoke(this, args);
            };
        }

        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            _currentId += 1;

            if (!NativeMethods.RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
            {
                throw new InvalidOperationException("Couldn't register the hot key.");
            }
        }

        public void UnRegisterHotKey()
        {
            for (var i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs>? KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            UnRegisterHotKey();
            _window.Dispose();
        }

        #endregion

        private class Window : NativeWindow, IDisposable
        {
            private static readonly int WM_HOTKEY = 0x0312;

            public Window()
            {
                CreateHandle();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg != WM_HOTKEY)
                    return;

                var key = (Keys)((int)m.LParam >> 16 & 0xFFFF);
                var modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);
                KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
            }

            private void CreateHandle()
            {
                CreateHandle(new CreateParams());
            }

            public event EventHandler<KeyPressedEventArgs>? KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                DestroyHandle();
            }

            #endregion
        }
    }
}
