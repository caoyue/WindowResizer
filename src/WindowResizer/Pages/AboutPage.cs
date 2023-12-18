using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Utils;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private const string ProjectLink = @"https://github.com/caoyue/WindowResizer";

        private void AboutPageInit()
        {

            if (App.IsRunningAsUwp)
            {
                StartupCheckBox.Checked = true;
                StartupCheckBox.Enabled= false;

                UpdateCheckBox.Checked = true;
                UpdateCheckBox.Enabled = false;
            }
            else
            {
                StartupCheckBox.Checked = SystemStartup.StartupStatus();
                StartupCheckBox.CheckedChanged += StartupCheckBox_CheckedChanged;

                UpdateCheckBox.Enabled = !ConfigFactory.PortableMode;
                UpdateCheckBox.Checked = ConfigFactory.Current.CheckUpdate && !ConfigFactory.PortableMode;
                UpdateCheckBox.CheckedChanged += UpdateCheckBox_CheckedChanged;
            }

            var portable = ConfigFactory.PortableMode ? " (portable)" : string.Empty;
            VersionLabel.Text = $"{App.Name} {Application.ProductVersion}{portable}";

            GithubLinkLabel.Text = ProjectLink;
            GithubLinkLabel.LinkClicked += LinkClicked;
        }

        private void StartupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StartupCheckBox.Checked)
            {
                SystemStartup.AddToStartup();
            }
            else
            {
                SystemStartup.RemoveFromStartup();
            }
        }

        private void UpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFactory.Current.CheckUpdate = UpdateCheckBox.Checked;
            ConfigFactory.Save();
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GithubLinkLabel.LinkVisited = true;
            Process.Start(ProjectLink);
        }

        private void ConfigExportBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export Config", AddExtension = true, DefaultExt = "json", FileName = $"{App.Name}.config.json"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel && saveFileDialog.FileName != "")
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    File.Delete(saveFileDialog.FileName);
                }

                File.Copy(ConfigFactory.ConfigPath, saveFileDialog.FileName);
            }
        }

        private void ConfigImportBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Import Config", AddExtension = true, DefaultExt = "json",
            };

            if (openFileDialog.ShowDialog() == DialogResult.Cancel || openFileDialog.FileName == "")
            {
                return;
            }

            var filePath = openFileDialog.FileName;
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Import failed, config file not found or deleted.");
                return;
            }

            try
            {
                ConfigFactory.Load(filePath);
                ConfigFactory.Save();

                ReRenderProfiles();
                OnProfileSwitch("Configs imported.");
            }
            catch (Exception exception)
            {
                Log.Append($"Import failed: {exception}");
                MessageBox.Show("Import failed, config file is not valid json.");
            }
        }

        private void OpenConfigButton_Click(object sender, EventArgs e)
        {
            var configFolder = ConfigFactory.PortableMode
                ? Application.StartupPath
                : Helper.GeApplicationDataPath();

            if (!Directory.Exists(configFolder))
            {
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = configFolder, FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
    }
}
