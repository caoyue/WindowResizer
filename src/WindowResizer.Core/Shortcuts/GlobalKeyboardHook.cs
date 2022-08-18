using System;
using System.Windows.Forms;
using static WindowResizer.Core.Shortcuts.NativeMethods;

namespace WindowResizer.Core.Shortcuts
{
    public sealed class GlobalKeyboardHook : IDisposable
    {
        private readonly KeyboardHookProc _hookProc;

        public event KeyEventHandler? KeyDown;

        public event KeyEventHandler? KeyUp;

        public GlobalKeyboardHook()
        {
            _hookProc = HookProc;
        }

        private IntPtr _hooked = IntPtr.Zero;

        public void Hook()
        {
            _hooked = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, IntPtr.Zero, 0);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(_hooked);
        }

        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code < 0)
            {
                return CallNextHookEx(_hooked, code, wParam, ref lParam);
            }

            Keys key = (Keys)lParam.vkCode;
            KeyEventArgs keyEventArgs = new KeyEventArgs(key);
            if (wParam is WM_KEYDOWN or WM_SYSKEYDOWN && (KeyDown != null))
            {
                KeyDown(this, keyEventArgs);
            }
            else if (wParam is WM_KEYUP or WM_SYSKEYUP && (KeyUp != null))
            {
                KeyUp(this, keyEventArgs);
            }

            return keyEventArgs.Handled ? 1 : CallNextHookEx(_hooked, code, wParam, ref lParam);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnHook();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }
    }
}
