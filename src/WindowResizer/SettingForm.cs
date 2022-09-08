using System;
using System.ComponentModel;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Controls;
using WindowResizer.Core.Shortcuts;
using WindowResizer.Utils;

namespace WindowResizer
{
    public partial class SettingForm : Form
    {
        private static KeyboardHook _hook;

        public SettingForm(KeyboardHook hook)
        {
            _hook = hook;

            InitializeComponent();
            InitWindow();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitWindow()
        {
            Font = new System.Drawing.Font(App.DefaultFontFamilyName, 9F);

            FormBorderStyle = FormBorderStyle.FixedSingle;

            FormClosing += SettingForm_Closing;
            Text = "WindowResizer - Setting";

            ProfileLabel.Text = $"Profile: {ConfigLoader.Config.ProfileName}";
            Helper.SetToolTip(ProfileLabel, "Click to rename profile.");

            HotkeysPageInit();
            ProcessesPageInit();
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

        public delegate void ConfigReloadEvent();

        public ConfigReloadEvent ConfigReload;

        private void ReloadConfig()
        {
            HotkeysPageInit();
            ConfigReload();
            ProcessesGrid.DataSource = ConfigLoader.Config.WindowSizes;
        }

        private void ProfileLabel_Click(object sender, EventArgs e)
        {
            using (Prompt prompt = new Prompt("Enter new profile name:", "Rename profile", this.Font))
            {
                string result = prompt.Result;
                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }

                if (ConfigLoader.RenameCurrentProfile(result.Trim()))
                {
                    ProfileLabel.Text = $"Profile: {ConfigLoader.Config.ProfileName}";
                    return;
                }

                Helper.ShowMessageBox("Name already taken.");
            }
        }
    }
}
