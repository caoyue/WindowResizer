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
    }
}
