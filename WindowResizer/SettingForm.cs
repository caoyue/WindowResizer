using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowResizer
{
    public partial class SettingForm : Form
    {
        private static Config _config;
        private static HotKeys _saveKeys;
        private static HotKeys _restoreKeys;
        private static KeyboardHook _hook;

        public SettingForm(Config config, KeyboardHook hook)
        {
            InitializeComponent();

            _config = config;
            _hook = hook;

            FormClosing += SettingForm_Closing;
            this.Text = "Setting";

            TextBoxInit();

            textBox1.KeyDown += (s, e) => textBox_KeyDown(s, e, textBox1, ref _saveKeys);
            textBox1.ReadOnly = true;

            textBox2.KeyDown += (s, e) => textBox_KeyDown(s, e, textBox2, ref _restoreKeys);
            textBox2.ReadOnly = true;

            textBox1.GotFocus += (s, e) => textBox_GotFocus(s, e, textBox1);
            textBox2.GotFocus += (s, e) => textBox_GotFocus(s, e, textBox2);

            textBox1.LostFocus += (s, e) => textBox_LostFocus(s, e, textBox1);
            textBox2.LostFocus += (s, e) => textBox_LostFocus(s, e, textBox2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {

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
            Restore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _config.SaveKey = _saveKeys;
            _config.RestoreKey = _restoreKeys;
            ConfigLoader.Save(_config);

            //hook
            _hook.UnRegisterHotKey();
            _hook.RegisterHotKey(_config.SaveKey.GetModifierKeys(), _config.SaveKey.GetKey());
            _hook.RegisterHotKey(_config.RestoreKey.GetModifierKeys(), _config.RestoreKey.GetKey());

            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Restore();
        }

        private void Restore()
        {
            TextBoxInit();
            Hide();
        }

        private void TextBoxInit()
        {
            textBox1.BackColor = Color.Blue;
            textBox1.ForeColor = Color.White;

            textBox2.BackColor = Color.Blue;
            textBox2.ForeColor = Color.White;

            textBox1.Text = _config.SaveKey.ToKeysString();
            textBox2.Text = _config.RestoreKey.ToKeysString();
        }
    }
}
