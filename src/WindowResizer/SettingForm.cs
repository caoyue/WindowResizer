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

            SetWindowTitle();

            HotkeysPageInit();
            ProcessesPageInit();
            ProfilesPageInit();
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

        private void SetWindowTitle()
        {
            Text = $"{nameof(WindowResizer)} - Setting  ::  {ConfigFactory.Current.ProfileName}";
        }

        public delegate void ConfigReloadEvent(string message);

        public ConfigReloadEvent ConfigReload;

        private void ReloadConfig(string message)
        {
            HotkeysPageReload();
            ConfigReload(message);
            ProcessesGrid.DataSource = ConfigFactory.Current.WindowSizes;
        }

        public void ShowFront()
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        public void SwitchTab(string tabName)
        {
            SettingTab.SelectedTab = SettingTab.TabPages[tabName];
        }
    }
}
