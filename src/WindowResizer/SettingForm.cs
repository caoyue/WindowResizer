using System;
using System.ComponentModel;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;

namespace WindowResizer
{
    public partial class SettingForm : Form
    {
        private static KeyboardHook _hook;

        public SettingForm(KeyboardHook hook)
        {
            InitializeComponent();

            _hook = hook;

            InitWindow();
        }

        private void InitWindow()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            FormClosing += SettingForm_Closing;
            Text = "WindowResizer - Setting";

            ShortcutPageInit();
            ConfigPageInit();
            AboutPageInit();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            BringToFront();
        }

        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void InitConfig(Config config)
        {
            WindowsGrid.DataSource = config.WindowSizes;
            SaveKeyLabel.Text = config.SaveKey.ToKeysString();
            RestoreKeyLabel.Text = config.RestoreKey.ToKeysString();
            RestoreAllKeyLabel.Text = config.RestoreAllKey.ToKeysString();
            DisableInFullScreenCheckBox.Checked = config.DisableInFullScreen;
        }
    }
}
