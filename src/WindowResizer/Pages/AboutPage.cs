using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WindowResizer.Configuration;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private const string ProjectLink = @"https://github.com/caoyue/WindowResizer";

        private void AboutPageInit()
        {
            UpdateCheckBox.Checked = ConfigLoader.Current.CheckUpdate;
            UpdateCheckBox.CheckedChanged += UpdateCheckBox_CheckedChanged;

            VersionLabel.Text = $"{nameof(WindowResizer)} {Application.ProductVersion}";

            GithubLinkLabel.Text = ProjectLink;
            GithubLinkLabel.LinkClicked += LinkClicked;

            PortableModeCheckBox.Checked = ConfigLoader.PortableMode;
            PortableModeCheckBox.CheckedChanged += PortableModeCheckBox_CheckedChanged;
        }

        private void UpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigLoader.Current.CheckUpdate = UpdateCheckBox.Checked;
            ConfigLoader.Save();
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
                Title = "Export Config", AddExtension = true, DefaultExt = "json", FileName = $"{nameof(WindowResizer)}.config.json"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel && saveFileDialog.FileName != "")
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    File.Delete(saveFileDialog.FileName);
                }

                File.Copy(ConfigLoader.ConfigPath, saveFileDialog.FileName);
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
                ConfigLoader.Load(filePath);
                ConfigLoader.Save();

                ReRenderProfiles();
                OnProfileSwitch("Configs imported.");
            }
            catch (Exception)
            {
                MessageBox.Show("Import failed, config file is not valid json.");
            }
        }

        private void PortableModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var desc = PortableModeCheckBox.Checked ? "Enter" : "Exit";
            const MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show($"{desc} portable mode?", "Confirm", messButton);
            if (dr == DialogResult.OK)
            {
                ConfigLoader.Move(PortableModeCheckBox.Checked);
            }
            else
            {
                PortableModeCheckBox.CheckedChanged -= PortableModeCheckBox_CheckedChanged;
                PortableModeCheckBox.Checked = !PortableModeCheckBox.Checked;
                PortableModeCheckBox.CheckedChanged += PortableModeCheckBox_CheckedChanged;
            }
        }
    }
}
