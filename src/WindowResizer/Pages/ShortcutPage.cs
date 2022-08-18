using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private static bool _isBinding;
        private static KeyBindControl _bindingControl;
        private readonly Hotkeys _hotKeys = new Hotkeys();

        private readonly GlobalKeyboardHook _globalHook = new GlobalKeyboardHook();
        private readonly List<Keys> _pressedKeys = new List<Keys>();

        private class KeyBindControl
        {
            public KeyBindControl(KeyBindType type, Button button, Label label)
            {
                Type = type;
                Button = button;
                Label = label;
            }

            public KeyBindType Type { get; }

            public Button Button { get; }

            public Label Label { get; }
        }

        private readonly List<KeyBindControl> _keyBindControls = new List<KeyBindControl>();

        private void ShortcutPageInit()
        {
            _globalHook.KeyDown += HookOnKeyDown;
            _globalHook.KeyUp += HookOnKeyUp;

            _keyBindControls.Add(new KeyBindControl(KeyBindType.Save, SaveKeyBtn, SaveKeyLabel));
            _keyBindControls.Add(new KeyBindControl(KeyBindType.Restore, RestoreKeyBtn, RestoreKeyLabel));
            _keyBindControls.Add(new KeyBindControl(KeyBindType.RestoreAll, RestoreAllKeyBtn, RestoreAllKeyLabel));

            foreach (var control in _keyBindControls)
            {
                control.Button.Click += (sender, e) => OnBindButtonClick(control, sender, e);
                control.Label.Text = GetLabelByType(control.Type);
            }

            DisableInFullScreenCheckBox.Checked = ConfigLoader.Config.DisableInFullScreen;
            DisableInFullScreenCheckBox.CheckedChanged += DisableInFullScreen_CheckedChanged;
        }

        private string GetLabelByType(KeyBindType type)
        {
            switch (type)
            {
                case KeyBindType.Save:
                    return ConfigLoader.Config.SaveKey.ToKeysString();

                case KeyBindType.Restore:
                    return ConfigLoader.Config.RestoreKey.ToKeysString();

                case KeyBindType.RestoreAll:
                    return ConfigLoader.Config.RestoreAllKey.ToKeysString();

                default:
                    return string.Empty;
            }
        }

        private void OnBindButtonClick(KeyBindControl control, object sender, EventArgs e)
        {
            if (_isBinding && _bindingControl.Button != control.Button)
            {
                _bindingControl.Button.PerformClick();
            }

            _bindingControl = control;

            OnKeyBindButtonClick(sender, e);
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

            _bindingControl.Label.Text = $"{_hotKeys.ToKeysString()} ...";
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
                _bindingControl.Button.PerformClick();
            }
        }

        private void OnKeyBindButtonClick(object sender, EventArgs _)
        {
            if (_isBinding)
            {
                OnKeyBindEnd();
            }
            else
            {
                OnKeyBindStart();
            }
        }

        private void OnKeyBindStart()
        {
            _isBinding = true;
            _hotKeys.Clear();
            _pressedKeys.Clear();
            _bindingControl.Button.Text = "Binding...";
            _bindingControl.Label.Text = "Waiting...";

            foreach (var control in _keyBindControls.Where(control => _bindingControl.Button != control.Button))
            {
                control.Button.Enabled = false;
            }

            _globalHook.Hook();
        }


        private void OnKeyBindEnd()
        {
            _globalHook.UnHook();

            _isBinding = false;
            _bindingControl.Button.Text = "Change";
            _bindingControl.Label.Text = _hotKeys.ToKeysString();

            foreach (var control in _keyBindControls)
            {
                control.Button.Enabled = true;
            }

            if (!IsKeyChanged(_bindingControl.Type))
            {
                return;
            }


            DialogResult dr = MessageBox.Show($"Set {_bindingControl.Type.ToString()} key to {_hotKeys.ToKeysString()}?", "Hotkey",
                MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                int keyId;
                try
                {
                    keyId = _hook.RegisterHotKey(_hotKeys.GetModifierKeys(), _hotKeys.GetKey());
                }
                catch (Exception e)
                {
                    App.ShowMessageBox(e.Message);
                    _bindingControl.Label.Text = GetLabelByType(_bindingControl.Type);
                    return;
                }

                if (App.RegisteredHotKeys.TryGetValue(_bindingControl.Type, out var oldKeyId))
                {
                    _hook.UnRegisterHotKey(oldKeyId);
                }

                App.RegisteredHotKeys[_bindingControl.Type] = keyId;

                switch (_bindingControl.Type)
                {
                    case KeyBindType.Save:
                        ConfigLoader.Config.SaveKey = _hotKeys;
                        break;

                    case KeyBindType.Restore:
                        ConfigLoader.Config.RestoreKey = _hotKeys;
                        break;

                    case KeyBindType.RestoreAll:
                        ConfigLoader.Config.RestoreAllKey = _hotKeys;
                        break;
                }

                ConfigLoader.Save();
            }
            else
            {
                _bindingControl.Label.Text = GetLabelByType(_bindingControl.Type);
            }
        }

        private bool IsKeyChanged(KeyBindType type)
        {
            switch (type)
            {
                case KeyBindType.Save:
                    return !_hotKeys.Equals(ConfigLoader.Config.SaveKey);

                case KeyBindType.Restore:
                    return !_hotKeys.Equals(ConfigLoader.Config.RestoreKey);

                case KeyBindType.RestoreAll:
                    return !_hotKeys.Equals(ConfigLoader.Config.RestoreAllKey);

                default:
                    return true;
            }
        }

        private void DisableInFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            ConfigLoader.Config.DisableInFullScreen = DisableInFullScreenCheckBox.Checked;
            ConfigLoader.Save();
        }
    }
}
