using System.Runtime.InteropServices;

namespace WindowResizer.Core.Theme;

internal static class NativeMethods
{
    [DllImport("uxtheme.dll", SetLastError = true, EntryPoint = "#95")]
    public static extern bool ShouldAppsUseDarkMode();

    [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
    public static extern bool ShouldSystemUseDarkMode();
}
