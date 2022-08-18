using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Core.Shortcuts;

namespace WindowResizer.Controls
{
    public class KeyBindButton : Button
    {
        public class KeyBindEventArgs : EventArgs
        {
            public Hotkeys HotKey { get; set; }
        }

        #region Key Bind

        private static bool _isBinding;
        private readonly Hotkeys _hotKeys = new Hotkeys();

        private readonly GlobalKeyboardHook _globalHook = new GlobalKeyboardHook();
        private readonly List<Keys> _pressedKeys = new List<Keys>();

        public event EventHandler OnKeyBindStart;
        public event EventHandler<KeyBindEventArgs> OnKeyBindEnd;

        public KeyBindButton()
        {
            _globalHook.KeyDown += HookOnKeyDown;
            _globalHook.KeyUp += HookOnKeyUp;
            Click += OnKeyBindButtonClick;
        }

        private void HookOnKeyDown(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            var key = args.KeyCode;
            if (_pressedKeys.Contains(key))
            {
                return;
            }

            _pressedKeys.Add(key);

            if (key.IsModifierKey())
            {
                _hotKeys.ModifierKeys.Add(key.ToKeyString());
            }
            else
            {
                _hotKeys.Key = key.ToKeyString();
            }
        }

        private void HookOnKeyUp(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            var key = args.KeyCode;
            if (_pressedKeys.Contains(key))
            {
                _pressedKeys.Remove(key);
            }

            if (_pressedKeys.Count == 0)
            {
                this.PerformClick();
            }
        }

        private void OnKeyBindButtonClick(object sender, EventArgs e)
        {
            if (_isBinding)
            {
                StopBind();
            }
            else
            {
                StartBind();
            }
        }

        private void StartBind()
        {
            _isBinding = true;
            _hotKeys.Clear();
            _pressedKeys.Clear();
            this.BackColor = Color.Red;
            _globalHook.Hook();

            OnKeyBindStart?.Invoke(this, EventArgs.Empty);
        }

        private void StopBind()
        {
            _isBinding = false;
            _globalHook.UnHook();
            this.BackColor = Color.Gray;
            this.Text = "Press to bind...";

            OnKeyBindEnd?.Invoke(this, new KeyBindEventArgs
            {
                HotKey = _hotKeys
            });
        }

        #endregion
    }
}
