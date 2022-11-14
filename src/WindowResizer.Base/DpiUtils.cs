using WindowResizer.Core.Utils;

namespace WindowResizer.Base;

public static class DpiUtils
{
    public static void SetDpiAware()
    {
        if (WindowsHelper.IsDpiAware)
        {
            SetProcessDPIAware();
        }
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();
}
