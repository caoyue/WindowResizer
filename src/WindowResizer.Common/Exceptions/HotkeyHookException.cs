using System;

namespace WindowResizer.Common.Exceptions;

public class HotkeyHookException : WindowResizerException
{
    public HotkeyHookException()
        : base("Unable to register hotkeys.")
    {
    }

    public HotkeyHookException(string message)
        : base(message)
    {
    }

    public HotkeyHookException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
