using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowResizer
{
    public partial class SettingForm : Form
    {
        private static HotKeys _saveKeys;
        private static HotKeys _restoreKeys;
        private static bool _disableInFullScreen;
        private static KeyboardHook _hook;

        public SettingForm(KeyboardHook hook)
        {
            InitializeComponent();

            _hook = hook;
            _saveKeys = ConfigLoader.config.SaveKey;
            _restoreKeys = ConfigLoader.config.RestoreKey;
            _disableInFullScreen = ConfigLoader.config.DisbaleInFullScreen;

            SaveKeysBox.BackColor = Color.Blue;
            SaveKeysBox.ForeColor = Color.White;

            RestoreKeysBox.BackColor = Color.Blue;
            RestoreKeysBox.ForeColor = Color.White;

            FormClosing += SettingForm_Closing;
            Text = "Setting";

            SaveKeysBox.KeyDown += (s, e) => textBox_KeyDown(s, e, SaveKeysBox, ref _saveKeys);
            SaveKeysBox.ReadOnly = true;

            RestoreKeysBox.KeyDown += (s, e) => textBox_KeyDown(s, e, RestoreKeysBox, ref _restoreKeys);
            RestoreKeysBox.ReadOnly = true;

            SaveKeysBox.GotFocus += (s, e) => textBox_GotFocus(s, e, SaveKeysBox);
            RestoreKeysBox.GotFocus += (s, e) => textBox_GotFocus(s, e, RestoreKeysBox);

            SaveKeysBox.LostFocus += (s, e) => textBox_LostFocus(s, e, SaveKeysBox);
            RestoreKeysBox.LostFocus += (s, e) => textBox_LostFocus(s, e, RestoreKeysBox);

            WindowsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", DataPropertyName = "name", HeaderText = "ExeName" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", DataPropertyName = "Title", HeaderText = "Title Ending" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Top", DataPropertyName = "Top", HeaderText = "Top" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Left", DataPropertyName = "Left", HeaderText = "Left" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Right", DataPropertyName = "Right", HeaderText = "Right" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Bottom", DataPropertyName = "Bottom", HeaderText = "Bottom" });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Rect", DataPropertyName = "Rect", Visible = false });

            WindowsGrid.DataSource = ConfigLoader.config.WindowSizes;
            SaveKeysBox.Text = _saveKeys.ToKeysString();
            RestoreKeysBox.Text = _restoreKeys.ToKeysString();
            checkBox1.Checked = ConfigLoader.config.DisbaleInFullScreen;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e,
            TextBox textBox, ref HotKeys hotKeys)
        {
            e.Handled = true;

            var ori = textBox.Text;
            textBox.Text = "Pressing...";
            textBox.BackColor = Color.OrangeRed;

            var keys = new List<string>();
            if (e.KeyCode != Keys.Back)
            {
                Keys modifierKeys = e.Modifiers;
                Keys pressedKey = e.KeyData ^ modifierKeys;

                if (modifierKeys != Keys.None && pressedKey != Keys.None)
                {
                    if (e.Control == true)
                    {
                        keys.Add("Ctrl");
                    }

                    if (e.Alt == true)
                    {
                        keys.Add("Alt");
                    }

                    if (e.Shift == true)
                    {
                        keys.Add("Shift");
                    }

                    hotKeys = new HotKeys()
                    {
                        ModifierKeys = keys.ToArray(),
                        Key = pressedKey.ToString()
                    };

                    textBox.Text = hotKeys.ToKeysString();
                }
                else
                {
                    textBox.Text = ori;
                }
            }
            else
            {
                textBox.Text = ori;
            }
            textBox.BackColor = Color.Blue;
            e.SuppressKeyPress = true;
        }

        private void textBox_GotFocus(object sender, EventArgs e, TextBox textBox)
        {
            textBox.BackColor = Color.OrangeRed;
        }

        private void textBox_LostFocus(object sender, EventArgs e, TextBox textBox)
        {
            textBox.BackColor = Color.Blue;
        }


        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// save settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            ConfigLoader.config.SaveKey = _saveKeys;
            ConfigLoader.config.RestoreKey = _restoreKeys;
            ConfigLoader.config.DisbaleInFullScreen = _disableInFullScreen;

            ConfigLoader.Save();

            //hook
            _hook.UnRegisterHotKey();
            _hook.RegisterHotKey(ConfigLoader.config.SaveKey.GetModifierKeys(), ConfigLoader.config.SaveKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.config.RestoreKey.GetModifierKeys(), ConfigLoader.config.RestoreKey.GetKey());

            Hide();
        }

        /// <summary>
        /// close settings panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _disableInFullScreen = checkBox1.Checked;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void ConfigExportBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export Config",
                AddExtension = true,
                DefaultExt = "json",
                FileName = Application.ProductName + "Config.json"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel && saveFileDialog.FileName != "")
            {
                if(File.Exists(saveFileDialog.FileName)) {
                    File.Delete(saveFileDialog.FileName);
                }
                File.Copy(ConfigLoader.ConfigPath, saveFileDialog.FileName);
            }
        }

        private void ConfigImportBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Import Config",
            };
            if(openFileDialog.ShowDialog() != DialogResult.Cancel && openFileDialog.FileName != "")
            {
                try
                {
                    var text = File.ReadAllText(openFileDialog.FileName);
                    var config = JsonConvert.DeserializeObject<Config>(text);
                    ConfigLoader.config = config;
                    _saveKeys = ConfigLoader.config.SaveKey;
                    _restoreKeys = ConfigLoader.config.RestoreKey;
                    _disableInFullScreen = ConfigLoader.config.DisbaleInFullScreen;
                    WindowsGrid.DataSource = ConfigLoader.config.WindowSizes;
                    SaveKeysBox.Text = _saveKeys.ToKeysString();
                    RestoreKeysBox.Text = _restoreKeys.ToKeysString();
                    checkBox1.Checked = ConfigLoader.config.DisbaleInFullScreen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Import failed! " + ex.Message);
                }
            }
        }
    }
}
