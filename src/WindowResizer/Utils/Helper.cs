using System;
using System.Drawing;
using System.Reflection;
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

        public static T Clone<T>(this T control)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(control, null), null);
                }
            }

            return instance;
        }

        public static bool IsWindows10Greater()
        {
            return Environment.OSVersion.Version >= new Version(10, 0, 17763);
        }
    }
}
