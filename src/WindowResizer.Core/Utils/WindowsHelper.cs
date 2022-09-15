using System;

namespace WindowResizer.Core.Utils;

public static class WindowsHelper
{
    public static bool IsDpiAware => Environment.OSVersion.Version.Major >= 6;
}
