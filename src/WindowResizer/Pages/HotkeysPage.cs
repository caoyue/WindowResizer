using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;
using WindowResizer.Utils;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private static bool _isRecording;
        private static HotkeysControl _recordingControl;

        private readonly Hotkeys _hotKeys = new Hotkeys();
        private readonly List<HotkeysControl> _keyRecordingControls = new List<HotkeysControl>();

        private readonly GlobalKeyboardHook _globalHook = new GlobalKeyboardHook();
        private readonly HashSet<Keys> _pressedKeys = new HashSet<Keys>();

        private class HotkeysControl
        {
            public HotkeysControl(HotkeysType type, Button button, Label label)
            {
                Type = type;
                Button = button;
                Label = label;
            }

            public HotkeysType Type { get; }

            public Button Button { get; }

            public Label Label { get; }
        }


        private void HotkeysPageInit()
        {
            _globalHook.KeyDown += HookOnKeyDown;
            _globalHook.KeyUp += HookOnKeyUp;

            _keyRecordingControls.Clear();
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.Save, SaveKeyBtn, SaveKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.Restore, RestoreKeyBtn, RestoreKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.SaveAll, SaveAllKeyBtn, SaveAllKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.RestoreAll, RestoreAllKeyBtn, RestoreAllKeyLabel));

            SetToolTips();

            foreach (var control in _keyRecordingControls)
            {
                control.Button.Click += (sender, e) => OnRecordButtonClick(control, sender, e);
                control.Button.LostFocus += Stop_Recording;
                control.Label.Text = GetLabelByType(control.Type);
                control.Label.Font = Helper.ChangeFontSize(SaveLabel.Font, 12F, FontStyle.Bold);
            }

            DisableInFullScreenCheckBox.Checked = ConfigLoader.Config.DisableInFullScreen;
            DisableInFullScreenCheckBox.CheckedChanged += DisableInFullScreen_CheckedChanged;
        }

        private void Stop_Recording(object sender, EventArgs e)
        {
            if (_isRecording)
            {
                OnKeyRecordEnd();
            }
        }

        private void OnRecordButtonClick(HotkeysControl control, object sender, EventArgs e)
        {
            if (_isRecording && _recordingControl.Button != control.Button)
            {
                _recordingControl.Button.PerformClick();
            }

            _recordingControl = control;

            OnKeyRecordButtonClick(sender, e);
        }

        private void DisableInFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            ConfigLoader.Config.DisableInFullScreen = DisableInFullScreenCheckBox.Checked;
            ConfigLoader.Save();
        }

        private void SetToolTips()
        {
            Helper.SetToolTip(SaveLabel, "Save foreground window size and position.");
            Helper.SetToolTip(RestoreLabel, "Restore foreground window size and position.");
            Helper.SetToolTip(SaveAllLabel, "Save all window size and position.");
            Helper.SetToolTip(RestoreAllLabel, "Restore all saved windows size and position.");
        }


        private void HookOnKeyDown(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            var key = args.KeyCode;

            if (!_pressedKeys.Add(key))
            {
                return;
            }

            if (key.IsModifierKey())
            {
                _hotKeys.ModifierKeys.Add(key.ToKeyString());
            }
            else
            {
                _hotKeys.Key = key.ToKeyString();
            }

            _recordingControl.Label.Text = $"{_hotKeys.ToKeysString()} ...";
        }

        private void HookOnKeyUp(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            var key = args.KeyCode;
            _pressedKeys.Remove(key);

            if (_pressedKeys.Count == 0)
            {
                _recordingControl.Button.PerformClick();
            }
        }

        private void OnKeyRecordButtonClick(object sender, EventArgs _)
        {
            if (_isRecording)
            {
                OnKeyRecordEnd();
            }
            else
            {
                OnKeyRecordStart();
            }
        }

        private void OnKeyRecordStart()
        {
            _isRecording = true;
            _hotKeys.Clear();
            _pressedKeys.Clear();
            _recordingControl.Button.Text = "Recording...";
            _recordingControl.Button.ForeColor = Color.White;
            _recordingControl.Button.BackColor = Color.Red;
            _recordingControl.Label.Text = "Waiting...";


            foreach (var control in _keyRecordingControls.Where(control => _recordingControl.Button != control.Button))
            {
                control.Button.Enabled = false;
            }

            _globalHook.Hook();
        }


        private void OnKeyRecordEnd()
        {
            _globalHook.UnHook();

            this.BringToFront();

            _isRecording = false;
            _recordingControl.Button.Text = "Edit";
            _recordingControl.Button.BackColor = SystemColors.ButtonFace;
            _recordingControl.Button.ForeColor = SystemColors.ControlText;
            _recordingControl.Label.Text = _hotKeys.ToKeysString();


            foreach (var control in _keyRecordingControls)
            {
                control.Button.Enabled = true;
            }

            if (!_hotKeys.IsValid() || !IsKeyChanged(_recordingControl.Type))
            {
                _recordingControl.Label.Text = GetLabelByType(_recordingControl.Type);
                return;
            }

            DialogResult dr = MessageBox.Show($"Set {_recordingControl.Type.ToString()} key to {_hotKeys.ToKeysString()}?", "Hotkey",
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
                    Helper.ShowMessageBox(e.Message);
                    _recordingControl.Label.Text = GetLabelByType(_recordingControl.Type);
                    return;
                }

                if (App.RegisteredHotKeys.TryGetValue(_recordingControl.Type, out var oldKeyId))
                {
                    _hook.UnRegisterHotKey(oldKeyId);
                }

                App.RegisteredHotKeys[_recordingControl.Type] = keyId;

                SetKeys(_recordingControl.Type, _hotKeys);

                ConfigLoader.Save();
            }
            else
            {
                _recordingControl.Label.Text = GetLabelByType(_recordingControl.Type);
            }
        }

        private static string GetLabelByType(HotkeysType type) =>
            GetKeys(type)?.ToKeysString() ?? string.Empty;

        private bool IsKeyChanged(HotkeysType type) =>
            !_hotKeys.Equals(GetKeys(type));

        private static void SetKeys(HotkeysType type, Hotkeys hotkeys) =>
            ConfigLoader.Config.SetKeys(type, hotkeys);

        private static Hotkeys GetKeys(HotkeysType type) =>
            ConfigLoader.Config.GetKeys(type);
    }
}
