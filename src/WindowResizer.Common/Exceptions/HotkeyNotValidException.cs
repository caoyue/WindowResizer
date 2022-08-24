using System;

namespace WindowResizer.Common.Exceptions;

public class HotkeyNotValidException : WindowResizerException
{
    public HotkeyNotValidException()
        : base("Not a valid hotkey.")
    {
    }

    public HotkeyNotValidException(string message)
        : base(message)
    {
    }

    public HotkeyNotValidException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
