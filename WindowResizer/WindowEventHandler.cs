using System;
using System.Windows.Automation;

namespace WindowResizer
{
    public class WindowEventHandler
    {
        private static AutomationEventHandler _eventHandler = null;

        public WindowEventHandler(Action<IntPtr> windowCreatedHandler)
        {
            _eventHandler = (sender, e) =>
            {
                AutomationElement src = sender as AutomationElement;
                if (src != null)
                {
                    Console.WriteLine("Class : " + src.Current.ClassName);
                    Console.WriteLine("Title : " + src.Current.Name);
                    Console.WriteLine("Handle: " + src.Current.NativeWindowHandle);
                    windowCreatedHandler(new IntPtr(src.Current.NativeWindowHandle));
                }
            };
        }

        public void AddWindowCreateHandle() =>
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowOpenedEvent,
                AutomationElement.RootElement,
                TreeScope.Subtree,
                _eventHandler
            );

        public void RemoveWindowCreateHandle()
        {
            Automation.RemoveAutomationEventHandler(
                WindowPattern.WindowOpenedEvent,
                AutomationElement.RootElement,
                _eventHandler
            );
        }
    }
}
