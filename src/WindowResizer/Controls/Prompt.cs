using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Utils;

namespace WindowResizer.Controls
{
    public partial class Prompt : Form
    {
        public Prompt(string text, string caption)
        {
            StartPosition = FormStartPosition.CenterParent;

            InitializeComponent();

            PromptText.Text = text;
            Init(caption);

            OkBtn.Click += (s, e) => this.Close();
        }

        public string Dialog()
        {
            return this.ShowDialog() == DialogResult.OK ? InputTextBox.Text : string.Empty;
        }

        private void Init(string caption)
        {
            this.Text = caption;

            PromptText.Font = Helper.ChangeFontSize(this.Font, 10F);
            InputTextBox.ForeColor = SystemColors.Highlight;
            InputTextBox.Font = Helper.ChangeFontSize(this.Font, 16F, FontStyle.Bold);

            this.Shown += (s, e) => InputTextBox.Focus();
        }
    }
}
