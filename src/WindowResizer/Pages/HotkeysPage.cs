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

            CheckBoxesInit();
        }

        #region checkboxes

        private void CheckBoxesInit()
        {
            DisableInFullScreenCheckBox.Checked = ConfigFactory.Current.DisableInFullScreen;
            DisableInFullScreenCheckBox.CheckedChanged += DisableInFullScreen_CheckedChanged;
            Helper.SetToolTip(DisableInFullScreenCheckBox, "Disable when current window is in fullscreen.");

            IncludeMinimizeCheckBox.Checked = ConfigFactory.Current.RestoreAllIncludeMinimized;
            IncludeMinimizeCheckBox.CheckedChanged += IncludeMinimized_CheckedChanged;
            Helper.SetToolTip(IncludeMinimizeCheckBox, "Include minimized windows when restoring all.");

            NotifyOnSavedCheckBox.Checked = ConfigFactory.Current.NotifyOnSaved;
            NotifyOnSavedCheckBox.CheckedChanged += NotifyOnSaved_CheckedChanged;
            Helper.SetToolTip(NotifyOnSavedCheckBox, "Show notification when save windows.");

            ResizeByTitleCheckbox.Checked = ConfigFactory.Current.EnableResizeByTitle;
            ResizeByTitleCheckbox.CheckedChanged += ResizeByTitle_CheckedChanged;
            Helper.SetToolTip(ResizeByTitleCheckbox, "Turn On/Off save/resize window according to the different titles of the same process.");

            AutoResizeDelayCheckbox.Checked = ConfigFactory.Current.EnableAutoResizeDelay;
            AutoResizeDelayCheckbox.CheckedChanged += AutoResizeDelay_CheckedChanged;
            Helper.SetToolTip(AutoResizeDelayCheckbox, "Turn On/Off auto resize delay.");
        }

        private void DisableInFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.DisableInFullScreen = DisableInFullScreenCheckBox.Checked;
            ConfigFactory.Save();
        }

        private void IncludeMinimized_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.RestoreAllIncludeMinimized = IncludeMinimizeCheckBox.Checked;
            ConfigFactory.Save();
        }

        private void NotifyOnSaved_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.NotifyOnSaved = NotifyOnSavedCheckBox.Checked;
            ConfigFactory.Save();
        }

        private void ResizeByTitle_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.EnableResizeByTitle = ResizeByTitleCheckbox.Checked;
            ConfigFactory.Save();

            ProcessesGrid_UpdateDataSource();
        }


        private void AutoResizeDelay_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.EnableAutoResizeDelay = AutoResizeDelayCheckbox.Checked;
            ConfigFactory.Save();

            ProcessesGrid_UpdateDataSource();
        }

        #endregion

        private void HotkeysPageReload()
        {
            _keyRecordingControls.Clear();
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.Save, SaveKeyBtn, SaveKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.Restore, RestoreKeyBtn, RestoreKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.SaveAll, SaveAllKeyBtn, SaveAllKeyLabel));
            _keyRecordingControls.Add(new HotkeysControl(HotkeysType.RestoreAll, RestoreAllKeyBtn, RestoreAllKeyLabel));

            SetToolTips();

            foreach (var control in _keyRecordingControls)
            {
                control.Label.Text = GetLabelByType(control.Type);
            }

            DisableInFullScreenCheckBox.Checked = ConfigFactory.Current.DisableInFullScreen;
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

                ConfigFactory.Save();
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
            ConfigFactory.Current.SetKeys(type, hotkeys);

        private static Hotkeys GetKeys(HotkeysType type) =>
            ConfigFactory.Current.GetKeys(type);
    }
}
