using System;
using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Utils;

namespace WindowResizer.Controls
{
    public class Prompt : IDisposable
    {
        private Form DialogPrompt { get; set; }

        public string Result { get; }

        private readonly Font _font;

        public Prompt(string text, string caption, Font font)
        {
            _font = font;
            Result = ShowDialog(text, caption);
        }

        //use a using statement
        private string ShowDialog(string text, string caption)
        {
            DialogPrompt = new Form
            {
                Text = caption,
                Width = 480,
                Height = 200,
                Font = _font,
                BackColor = SystemColors.InactiveBorder,
                ForeColor = SystemColors.ControlText,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                MaximizeBox = false,
                MinimizeBox = false,
            };
            var textLabel = new Label
            {
                Left = 30,
                Top = 12,
                Width = 400,
                Height = 30,
                Text = text,
                ForeColor = SystemColors.ControlText,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            var textBox = new TextBox
            {
                Left = 30,
                Top = 50,
                Width = 400,
                ForeColor = SystemColors.Highlight,
                Font = Helper.ChangeFontSize(_font, 14F, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Center,
            };
            var confirmation = new Button
            {
                Text = "Ok",
                Left = 180,
                Width = 100,
                Height = 30,
                Top = 105,
                BackColor = SystemColors.ButtonFace,
                ForeColor = SystemColors.ControlText,
                FlatStyle = FlatStyle.Standard,
                DialogResult = DialogResult.OK
            };
            confirmation.Click += (sender, e) => { DialogPrompt.Close(); };
            DialogPrompt.Controls.Add(textBox);
            DialogPrompt.Controls.Add(confirmation);
            DialogPrompt.Controls.Add(textLabel);
            DialogPrompt.AcceptButton = confirmation;

            return DialogPrompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }

        public void Dispose()
        {
            DialogPrompt?.Dispose();
        }
    }
}
