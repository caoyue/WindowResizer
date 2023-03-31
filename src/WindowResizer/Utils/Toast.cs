using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WindowResizer.Utils
{
    internal static class Toast
    {
        private const string ActionKey = "Action";

        public static void ShowToast(
            string message,
            string title = "",
            int expired = 3000,
            NotifyIcon tray = null,
            ActionLevel actionLevel = ActionLevel.Info,
            ActionType actionType = ActionType.None
        )
        {
            if (Helper.IsWindows10Greater())
            {
                Send(title, message, actionType, expired);
            }
            else if (tray != null)
            {
                var icon = (ToolTipIcon)((int)actionLevel);
                tray.ShowBalloonTip(expired, title, message, icon);
            }
        }

        public static void Send(string title, string content, ActionType actionType, int expired = 3000)
        {
            var imageUri = Path.GetFullPath(@"Resources\AppIcon.png");

            var builder =
                new ToastContentBuilder()
                    .AddText(title)
                    .AddText(content);

            switch (actionType)
            {
                case ActionType.OpenProcessSetting:
                {
                    builder.AddButton(new ToastButton()
                                      .SetContent("Open Setting")
                                      .AddArgument(ActionKey, actionType)
                                      .SetBackgroundActivation());
                    break;
                }
            }

            builder
                .AddAppLogoOverride(new Uri(imageUri))
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMilliseconds(expired);
                });
        }

        public static void OnStart(Action<ActionType> action)
        {
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                var a = args.FirstOrDefault(i => i.Key.Equals(ActionKey));
                if (!string.IsNullOrEmpty(a.Value))
                {
                    action?.Invoke((ActionType)int.Parse(a.Value));
                }
            };
        }

        public static void OnStop()
        {
            ToastNotificationManagerCompat.Uninstall();
        }

        public enum ActionType
        {
            None,
            OpenProcessSetting,
        }

        public enum ActionLevel
        {
            None,
            Info,
            Warning,
            Error,
        }
    }
}
