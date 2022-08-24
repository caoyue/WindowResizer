using System;

namespace WindowResizer.Common.Exceptions;

public class WindowResizerException : Exception
{
    public WindowResizerException(string message)
        : base(message)
    {
    }

    public WindowResizerException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
