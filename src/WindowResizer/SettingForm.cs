using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;

namespace WindowResizer
{
    public partial class SettingForm : Form
    {
        private static HotKeys _saveKeys;
        private static HotKeys _restoreKeys;
        private static HotKeys _restoreAllKeys;
        private static bool _disableInFullScreen;
        private static KeyboardHook _hook;

        public SettingForm(KeyboardHook hook)
        {
            InitializeComponent();

            _hook = hook;
            _saveKeys = ConfigLoader.Config.SaveKey;
            _restoreKeys = ConfigLoader.Config.RestoreKey;
            _restoreAllKeys = ConfigLoader.Config.RestoreAllKey;
            _disableInFullScreen = ConfigLoader.Config.DisableInFullScreen;

            InitWindow();
            InitKeyTextBox();
            InitDataGrid();
            About();
        }

        #region Init Setting Window

        private void InitWindow()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            FormClosing += SettingForm_Closing;
            Text = "WindowResizer - Setting";
        }

        #endregion

        #region Init Config Grid

        private void InitDataGrid()
        {
            WindowsGrid.AllowUserToAddRows = false;
            WindowsGrid.RowTemplate.Height = 50;
            WindowsGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            WindowsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "ExeName",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = Color.Blue
                },
                FillWeight = 15,
                DisplayIndex = 0,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                DataPropertyName = "Title",
                HeaderText = "Title",
                Resizable = DataGridViewTriState.True,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                FillWeight = 35,
                DisplayIndex = 1,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Top",
                DataPropertyName = "Top",
                HeaderText = "Top",
                FillWeight = 8,
                DisplayIndex = 2,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Left",
                DataPropertyName = "Left",
                HeaderText = "Left",
                FillWeight = 8,
                DisplayIndex = 3,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Right",
                DataPropertyName = "Right",
                HeaderText = "Right",
                FillWeight = 8,
                DisplayIndex = 4,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Bottom",
                DataPropertyName = "Bottom",
                HeaderText = "Bottom",
                FillWeight = 8,
                DisplayIndex = 5,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rect",
                DataPropertyName = "Rect",
                Visible = false,
            });

            WindowsGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "AutoResize",
                DataPropertyName = "AutoResize",
                HeaderText = "Auto",
                FillWeight = 8,
                DisplayIndex = 6,
            });

            WindowsGrid.Columns.Add(new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Remove",
                Name = "Remove",
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle =
                {
                    ForeColor = Color.Blue
                },
                FillWeight = 10,
                DisplayIndex = 7,
            });

            foreach (DataGridViewColumn col in WindowsGrid.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font =
                    new Font("Microsoft YaHei UI", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
            }

            WindowsGrid.AutoGenerateColumns = false;
            WindowsGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;

            WindowsGrid.ShowCellToolTips = true;
            WindowsGrid.CellFormatting += WindowsGrid_CellFormatting;
            WindowsGrid.CellClick += WindowsGrid_CellClick;
            WindowsGrid.CellValueChanged += WindowsGrid_CellValueChanged;
        }

        #endregion

        #region Init Key Bind

        private void InitKeyTextBox()
        {
            SaveKeysBox.BackColor = Color.Blue;
            SaveKeysBox.ForeColor = Color.White;

            RestoreKeysBox.BackColor = Color.Blue;
            RestoreKeysBox.ForeColor = Color.White;

            RestoreAllKeyBox.BackColor = Color.Blue;
            RestoreAllKeyBox.ForeColor = Color.White;

            SaveKeysBox.KeyDown += (s, e) => textBox_KeyDown(s, e, SaveKeysBox,
                ref _saveKeys);
            SaveKeysBox.ReadOnly = true;

            RestoreKeysBox.KeyDown += (s, e) => textBox_KeyDown(s, e, RestoreKeysBox, ref _restoreKeys);
            RestoreKeysBox.ReadOnly = true;

            RestoreAllKeyBox.KeyDown += (s, e) => textBox_KeyDown(s, e, RestoreAllKeyBox, ref _restoreAllKeys);
            RestoreAllKeyBox.ReadOnly = true;

            SaveKeysBox.GotFocus += (s, e) => textBox_GotFocus(s, e, SaveKeysBox);
            RestoreKeysBox.GotFocus += (s, e) => textBox_GotFocus(s, e, RestoreKeysBox);
            RestoreAllKeyBox.GotFocus += (s, e) => textBox_GotFocus(s, e, RestoreAllKeyBox);

            SaveKeysBox.LostFocus += (s, e) => textBox_LostFocus(s, e, SaveKeysBox);
            RestoreKeysBox.LostFocus += (s, e) => textBox_LostFocus(s, e, RestoreKeysBox);
            RestoreAllKeyBox.LostFocus += (s, e) => textBox_LostFocus(s, e, RestoreAllKeyBox);

            SaveKeysBox.Text = _saveKeys.ToKeysString();
            RestoreKeysBox.Text = _restoreKeys.ToKeysString();
            RestoreAllKeyBox.Text = _restoreAllKeys.ToKeysString();
            checkBox1.Checked = ConfigLoader.Config.DisableInFullScreen;

            checkBox2.Checked = ConfigLoader.PortableMode;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
        }

        #endregion

        #region About

        private const string ProjectLink = @"https://github.com/caoyue/WindowResizer";

        private void About()
        {
            label4.Text = $"WindowResizer {Application.ProductVersion}";

            linkLabel1.Text = ProjectLink;
            linkLabel1.LinkClicked += LinkClicked;
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start(ProjectLink);
        }

        #endregion

        private void WindowsGrid_CellValueChanged(object sender,
            DataGridViewCellEventArgs e)
        {
        }

        private void WindowsGrid_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null &&
                (e.ColumnIndex == WindowsGrid.Columns["Name"]?.Index ||
                    e.ColumnIndex == WindowsGrid.Columns["Title"]?.Index))
            {
                DataGridViewCell cell =
                    WindowsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = cell.Value.ToString();
            }
        }

        private void WindowsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WindowsGrid.Columns["Remove"]?.Index &&
                e.RowIndex >= 0 &&
                e.RowIndex < ConfigLoader.Config.WindowSizes.Count)
            {
                ConfigLoader.Config.WindowSizes.RemoveAt(e.RowIndex);
            }
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
                    if (e.Control)
                    {
                        keys.Add("Ctrl");
                    }

                    if (e.Alt)
                    {
                        keys.Add("Alt");
                    }

                    if (e.Shift)
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
            ReloadConfig();
            Hide();
        }

        /// <summary>
        /// save settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            ConfigLoader.Config.SaveKey = _saveKeys;
            ConfigLoader.Config.RestoreKey = _restoreKeys;
            ConfigLoader.Config.RestoreAllKey = _restoreAllKeys;
            ConfigLoader.Config.DisableInFullScreen = _disableInFullScreen;

            ConfigLoader.Save();

            //hook
            _hook.UnRegisterHotKey();
            _hook.RegisterHotKey(ConfigLoader.Config.SaveKey.GetModifierKeys(), ConfigLoader.Config.SaveKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.Config.RestoreKey.GetModifierKeys(), ConfigLoader.Config.RestoreKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.Config.RestoreAllKey.GetModifierKeys(), ConfigLoader.Config.RestoreAllKey.GetKey());

            Hide();
        }

        /// <summary>
        /// close settings panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ReloadConfig();
            Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _disableInFullScreen = checkBox1.Checked;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            BringToFront();
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
                Title = "Import Config",
            };
            if (openFileDialog.ShowDialog() != DialogResult.Cancel && openFileDialog.FileName != "")
            {
                try
                {
                    var text = File.ReadAllText(openFileDialog.FileName);
                    var config = JsonConvert.DeserializeObject<Config>(text);
                    if (config == null)
                    {
                        MessageBox.Show("Import failed, config file is not valid json.");
                        return;
                    }

                    ConfigLoader.Config = config;
                    _saveKeys = ConfigLoader.Config.SaveKey;
                    _restoreKeys = ConfigLoader.Config.RestoreKey;
                    _restoreAllKeys = ConfigLoader.Config.RestoreAllKey;
                    _disableInFullScreen = ConfigLoader.Config.DisableInFullScreen;
                    WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;
                    SaveKeysBox.Text = _saveKeys.ToKeysString();
                    RestoreKeysBox.Text = _restoreKeys.ToKeysString();
                    RestoreAllKeyBox.Text = _restoreAllKeys.ToKeysString();
                    checkBox1.Checked = ConfigLoader.Config.DisableInFullScreen;
                }
                catch (Exception)
                {
                    MessageBox.Show("Import failed, config file is not valid json.");
                }
            }
        }

        private void ReloadConfig()
        {
            ConfigLoader.Load();
            WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            var desc = checkBox2.Checked ? "Enter" : "Exit";
            const MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show($"{desc} portable mode?", "Confirm", messButton);
            if (dr == DialogResult.OK)
            {
                ConfigLoader.Move(checkBox2.Checked);
            }
            else
            {
                checkBox2.CheckedChanged -= checkBox2_CheckedChanged;
                checkBox2.Checked = !checkBox2.Checked;
                checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            }
        }
    }
}
