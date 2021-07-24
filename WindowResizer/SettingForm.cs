using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            FormBorderStyle = FormBorderStyle.FixedSingle;

            _hook = hook;
            _saveKeys = ConfigLoader.Config.SaveKey;
            _restoreKeys = ConfigLoader.Config.RestoreKey;
            _disableInFullScreen = ConfigLoader.Config.DisbaleInFullScreen;

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

            SaveKeysBox.Text = _saveKeys.ToKeysString();
            RestoreKeysBox.Text = _restoreKeys.ToKeysString();
            checkBox1.Checked = ConfigLoader.Config.DisbaleInFullScreen;

            InitDataGrid();
        }

        private void InitDataGrid()
        {
            WindowsGrid.AllowUserToAddRows = false;
            WindowsGrid.RowTemplate.Height = 50;
            WindowsGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            WindowsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "ExeName",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 300,
                DefaultCellStyle = new DataGridViewCellStyle {ForeColor = Color.Blue}
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                DataPropertyName = "Title",
                HeaderText = "Title",
                Resizable = DataGridViewTriState.True,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 600,
                DefaultCellStyle = new DataGridViewCellStyle {Alignment = DataGridViewContentAlignment.MiddleLeft}
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Top", DataPropertyName = "Top", HeaderText = "Top"
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Left", DataPropertyName = "Left", HeaderText = "Left"
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Right", DataPropertyName = "Right", HeaderText = "Right"
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Bottom", DataPropertyName = "Bottom", HeaderText = "Bottom"
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rect", DataPropertyName = "Rect", Visible = false
            });

            var rmBtn = new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Remove",
                Name = "Remove",
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = {ForeColor = Color.Blue}
            };
            WindowsGrid.Columns.Add(rmBtn);

            foreach (DataGridViewColumn col in WindowsGrid.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font =
                    new Font("Microsoft YaHei UI", 26F, FontStyle.Bold, GraphicsUnit.Pixel);
            }

            WindowsGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;

            WindowsGrid.ShowCellToolTips = true;
            WindowsGrid.CellFormatting += WindowsGrid_CellFormatting;
            WindowsGrid.CellClick += WindowsGrid_CellClick;
            WindowsGrid.CellValueChanged += WindowsGrid_CellValueChanged;
        }

        private void WindowsGrid_CellValueChanged(object sender,
            DataGridViewCellEventArgs e)
        {
        }

        private void WindowsGrid_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && (e.ColumnIndex == WindowsGrid.Columns["Name"]?.Index ||
                                    e.ColumnIndex == WindowsGrid.Columns["Title"]?.Index))
            {
                DataGridViewCell cell =
                    WindowsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = cell.Value.ToString();
            }
        }

        private void WindowsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WindowsGrid.Columns["Remove"]?.Index
                && e.RowIndex >= 0 && e.RowIndex < ConfigLoader.Config.WindowSizes.Count)
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

                    hotKeys = new HotKeys() {ModifierKeys = keys.ToArray(), Key = pressedKey.ToString()};

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
            ConfigLoader.Config.DisbaleInFullScreen = _disableInFullScreen;

            ConfigLoader.Save();

            //hook
            _hook.UnRegisterHotKey();
            _hook.RegisterHotKey(ConfigLoader.Config.SaveKey.GetModifierKeys(), ConfigLoader.Config.SaveKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.Config.RestoreKey.GetModifierKeys(),
                ConfigLoader.Config.RestoreKey.GetKey());

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
            OpenFileDialog openFileDialog = new OpenFileDialog {Title = "Import Config",};
            if (openFileDialog.ShowDialog() != DialogResult.Cancel && openFileDialog.FileName != "")
            {
                try
                {
                    var text = File.ReadAllText(openFileDialog.FileName);
                    var config = JsonConvert.DeserializeObject<Config>(text);
                    ConfigLoader.Config = config;
                    _saveKeys = ConfigLoader.Config.SaveKey;
                    _restoreKeys = ConfigLoader.Config.RestoreKey;
                    _disableInFullScreen = ConfigLoader.Config.DisbaleInFullScreen;
                    WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;
                    SaveKeysBox.Text = _saveKeys.ToKeysString();
                    RestoreKeysBox.Text = _restoreKeys.ToKeysString();
                    checkBox1.Checked = ConfigLoader.Config.DisbaleInFullScreen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Import failed! " + ex.Message);
                }
            }
        }

        private void ReloadConfig()
        {
            ConfigLoader.Load();
            WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;
        }
    }
}
