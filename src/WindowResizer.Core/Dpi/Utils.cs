using System;
using static WindowResizer.Core.Dpi.NativeMethods;

namespace WindowResizer.Core.Dpi;

public static class Utils
{
    public static bool IsDpiAwareness => Environment.OSVersion.Version >= new Version(6, 3, 0);

    public static void SetDpiAwareness()
    {
        // win 8.1 added support for per monitor dpi
        if (IsDpiAwareness)
        {
            // win 10 creators update added support for per monitor v2
            if (Environment.OSVersion.Version >= new Version(10, 0, 15063))
            {
                SetProcessDpiAwarenessContext((int)DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
            }
            else
            {
                SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            }
        }
        else
        {
            SetProcessDPIAware();
        }
    }
}
