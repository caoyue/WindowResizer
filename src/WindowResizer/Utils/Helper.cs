using System.Drawing;
using System.Windows.Forms;

namespace WindowResizer.Utils
{
    internal static class Helper
    {
        public static Font ChangeFontSize(Font font, float fontSize, FontStyle style = FontStyle.Regular)
        {
            if (font != null)
            {
                font = new Font(font.Name, fontSize, style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
            }

            return font;
        }

        public static void ShowMessageBox(string message, MessageBoxIcon icon = MessageBoxIcon.Error, string title = nameof(WindowResizer))
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        public static void SetToolTip(Control control, string message)
        {
            var toolTip = new ToolTip();
            // toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.IsBalloon = true;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(control, message);
        }
    }
}
